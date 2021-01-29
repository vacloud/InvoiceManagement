using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SourceCode.Services;
using SourceCode.Models;
using SourceCode.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SourceCode.Controllers
{
    // Webmaster Controller
    //[Authorize]
    public class WebMasterController : Controller
    {
        private readonly IGeneralService _GeneralService;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly SignInManager<IdentityUser> _SignInManager;
        private readonly ICurrencyService _CurrencyService;
        private readonly IConfiguration _Config;
        private readonly IHostingEnvironment _Evn;
        private readonly IEmailService _EmailService;
        int PageSize = 0;
        public List<SelectListItem> TFList = new List<SelectListItem> {
            new SelectListItem { Text="True" , Value="True"} ,
        new SelectListItem { Text="False" , Value="False"}
        };

        #region Constructor With DI (Dependecy Injection)

        public WebMasterController(IGeneralService _generalService , 
             UserManager<IdentityUser> _userManager,
            IHttpContextAccessor _httpContextAccessor, 
            SignInManager<IdentityUser> _signInManager,
            ICurrencyService _currencyService , 
            IConfiguration _config,
            IHostingEnvironment _evn,
            IEmailService _emailService
            
            )
        {
            _GeneralService = _generalService;
            _UserManager = _userManager;
            _HttpContextAccessor = _httpContextAccessor;
            _SignInManager = _signInManager;
            _CurrencyService = _currencyService;
            _Config = _config;
            _Evn = _evn;
            _EmailService = _emailService;
            _EmailService = _emailService;
            PageSize = _GeneralService.GetGeneralSettings().PageSize;
        }

        #endregion
       

        #region LoginPanel
        // function is used for login, in to the system/portal 
        [AllowAnonymous]
        [HttpGet]
        public IActionResult LoginPanel()
        {
            if(_HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginPanel(Login model)
        {
            if(ModelState.IsValid)
            {
                var result = await _SignInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard");

                }
                else
                {
                    ModelState.AddModelError("", "Please Enter Correct Username & Password");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
            
        }


        #endregion

        #region Logout

        // helps in Logging out 
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _SignInManager.SignOutAsync();
                return RedirectToAction("LoginPanel", "WebMaster");
            }
            catch
            {
                return RedirectToAction("LoginPanel", "WebMaster");
            }
          

        }

        #endregion

        #region Dashboard
        // Admin Dashbaord
        public IActionResult Dashboard(string msg)
        {

            return View();

        }
        #endregion

        #region GeneralSettings
        // Update General Settings of the website
        [HttpGet]
        public IActionResult GeneralSettings()
        {
            ViewBag.MsgShow = TempData["Error"];
            var model = new GeneralSettings();
            model.GeneralModel = _GeneralService.GetGeneralSettings();
            model.CurrencyList = _CurrencyService.AllCurrencies();
            return View(model);
        }

        [HttpPost]
        public IActionResult GeneralSettings(GeneralSettings model)
        {

            if (ModelState.IsValid)
            {
                // If Image is selected than upadted LogoUrl
                if (model.LogoImage!=null && model.LogoImage.Length >0)
                {
                    // get unique File Name in this variable
                    string NewFileName = "";
                    // get uniquelogo path to save in this variable
                    string LogoUrlPath = "";
                    string PathToAdd = GetPath(model.LogoImage.FileName, out NewFileName,out LogoUrlPath);

                    // if directory doesn't exists , create it
                    if (!Directory.Exists(PathToAdd))
                    {
                        Directory.CreateDirectory(PathToAdd);
                      
                    }
                    if(System.IO.File.Exists(_Evn.WebRootPath+"/"+ _GeneralService.GetGeneralSettings().LogoUrl))
                    {
                        System.IO.File.Delete(_Evn.WebRootPath + "/" + _GeneralService.GetGeneralSettings().LogoUrl);
                    }
                    using (var stream = new FileStream(PathToAdd+"/"+ NewFileName, FileMode.Create))
                    {
                        model.LogoImage.CopyTo(stream);
                    }
                    model.GeneralModel.LogoUrl = LogoUrlPath;
                }
                else
                {
                    
                    model.GeneralModel.LogoUrl= _GeneralService.GetGeneralSettings().LogoUrl;
                }
                General generalData = _GeneralService.UpdateGeneralSettings(model.GeneralModel);
                TempData["Error"] = "Details Updated Successfully.";
                return RedirectToAction("GeneralSettings");
            }
            else
            {
                return View(model);
            }
        }

        #endregion

        #region ProfileSettings
        // Update you password here
        [HttpGet]
        public IActionResult ProfileSettings()
        {
            ViewBag.MsgShow = TempData["Error"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProfileSettings(UpdatePassword model)
        {
            if (_HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    string UserID = _UserManager.GetUserId(_HttpContextAccessor.HttpContext.User);

                    var User = await _UserManager.FindByIdAsync(UserID);

                    var result = await _UserManager.ChangePasswordAsync(User, model.OldPassword.Trim(), model.NewPassword.Trim());

                    if (result.Succeeded)
                    {
                        TempData["Error"] = "Details Updated Successfully.";
                        return RedirectToAction("ProfileSettings");
                    }
                    else
                    {
                        AddErrors(result);
                        return View(model);
                    }


                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                
                return View("LoginPanel");
            }


        }

        #endregion

        #region Currency Tasks

        [HttpGet]
        public IActionResult CurrencyMaster(string msg,int? page,int ID)
        {
            // For Using Paging we installed X.PagedList.Mvc.Core package.
            // For more details see link : https://github.com/dncuug/X.PagedList
            // Using X.PagedList; Namespace is need to call for the usage.
            int pageNumber = page ?? 1;
            ViewBag.MsgShow = msg;
            CurrencyVM model = new CurrencyVM();
           
            model.CurrencyList = _CurrencyService.AllCurrenciesList().ToPagedList(pageNumber, PageSize);
            if(ID!=0)
            {
                var data = _CurrencyService.GetCurrencyByID(ID);
                Currency Curr = new Currency
                {
                    ID = data.ID,
                    Symbol= data.Symbol
                };
                model.Currency = Curr;
            }
           
            return View(model);
            
        }

        [HttpPost]
        public IActionResult CurrencyMaster(CurrencyVM model, int? page)
        {
            int pageNumber = page ?? 1;

            int PageSize = _GeneralService.GetGeneralSettings().PageSize;

            model.CurrencyList = _CurrencyService.AllCurrenciesList().ToPagedList(pageNumber, PageSize);

            // We used Currency.ID on view, so it will send null value to Currency.ID
            // We need to convert null to zero so below line of code is used.
            model.Currency.ID = Convert.ToInt32(model.Currency.ID);
            // we do not use Currency.ID on view  (which basically used by update function) 
            // than above line of code is not required.


            // FOR INSERT
            if (model.Currency.ID == 0 )
            {

                // Need to remove modeal state of ID else it will give model state invalid error
                ModelState.Remove("Currency.ID");
                // this is because we used hidden field  Currency.ID on view, 
                // if we do not use this on view than above line is not required
               

                if (ModelState.IsValid)
                {
                    Currency curr = new Currency
                    {
                        Symbol = model.Currency.Symbol
                    };
                    model.Currency = _CurrencyService.AddCurrency(curr);
                    TempData["Error"] = "Details Added Successfully.";
                    return RedirectToAction("CurrencyMaster", new {  page = page });
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                // FOR UPDATE
                if (ModelState.IsValid)
                {
                    model.Currency = _CurrencyService.UpdateCurrency(model.Currency);
                    TempData["Error"] = "Details Updated Successfully.";
                    return RedirectToAction("CurrencyMaster", new {  page = page });
                }
                else
                {
                    return View(model);
                }
            }
            
            
        }

        [HttpPost]
        public IActionResult CurrencyMasterDelete(int ID, int? page)
        {
            var data = _CurrencyService.GetCurrencyByID(ID);
            _CurrencyService.DeleteCurrency(data);
            TempData["Error"] = "Details Deleted Successfully.";
            return RedirectToAction("CurrencyMaster", new { page = page });

        }

        #endregion

        #region EmailSettings
        // Update Email Settings of the website
        [HttpGet]
        public IActionResult EmailSettingUpdate()
        {
            ViewBag.MsgShow = TempData["Error"] ;
            var model = new EmailSettingsVM();
            model.EmailSettings = _EmailService.GetEmailSettings();
            model.TFList = TFList;
            return View(model);
        }

        [HttpPost]
        public IActionResult EmailSettingUpdate(EmailSettingsVM model)
        {

            if (ModelState.IsValid)
            {
                
                EmailSettings generalData =_EmailService.UpdateEmailSettings(model.EmailSettings);
                TempData["Error"] = "Details Updated Successfully.";
                return RedirectToAction("EmailSettingUpdate");
            }
            else
            {
                return View(model);
            }
        }

        #endregion

        #region CommonTasks

        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Code.Contains("PasswordMismatch"))
                {
                    ModelState.AddModelError("", "Old Password is wrong.");
                }
                else
                {
                    ModelState.AddModelError("", error.Description);

                }
            }
        }

        public string GetPath(string fileName,out string NewFileName,out string LogoUrlPath)
        {
            //add year & month path
            string PathToSave ="/StaticFiles/" + System.DateTime.UtcNow.Year.ToString() + "/" + System.DateTime.UtcNow.Month.ToString() +"/";
            // creating unique filename
            string uniqueIdentifier = System.DateTime.UtcNow.ToString("dddd-dd-MMMM-yyyy_HH-mm-ss")+"_"+ fileName;
            // returing new file name in out parameter
            NewFileName = uniqueIdentifier;
            // return Logo Url Path
            LogoUrlPath = PathToSave + uniqueIdentifier;
            return _Evn.WebRootPath + PathToSave;
        }
       
        #endregion

    }
}