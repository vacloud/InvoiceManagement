using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using SourceCode.Services;
using SourceCode.ViewModels;
using X.PagedList;

namespace SourceCode.Controllers
{
    // Invoice & Billing Controller
    //[Authorize]
    public class IBController : Controller
    {
        public static string  LayoutNameGlobal="";
        int PageSize = 0;
        private readonly IGeneralService _GeneralService;
        private readonly IProductService _ProductService;
        private readonly ICustomerService _CustomerService;
        private readonly IInvoiceService _InvoiceService;
        private readonly IEmailService _EmailService;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        public List<SelectListItem> StatusList = new List<SelectListItem> {
            new SelectListItem { Text="Due" , Value="Due"} ,
        new SelectListItem { Text="Paid" , Value="Paid"}
        };
       
        public IBController(IGeneralService _generalService,
            IProductService _productService,
            ICustomerService _customerService,
            IInvoiceService _invoiceService,
            IEmailService _emailService,
            IHttpContextAccessor _httpContextAccessor
            )
        {
            _GeneralService = _generalService;
            _ProductService = _productService;
            _CustomerService = _customerService;
            _InvoiceService = _invoiceService;
            _EmailService = _emailService;
            _HttpContextAccessor = _httpContextAccessor;
            PageSize = _GeneralService.GetGeneralSettings().PageSize;
        }
        [HttpGet]
        public IActionResult SelectLayout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SelectLayout(string ln)
        {
            LayoutNameGlobal = ln;
            return RedirectToAction("CreateInvoice");
        }

        [HttpGet]
        public IActionResult CreateInvoice()
        {
            ViewBag.MsgShow = TempData["Error"];
           Invoice model = new Invoice();
            model.AllStatusList = StatusList;
            model.LayoutName = LayoutNameGlobal;
            model.AllProducts = _ProductService.AllProductListForDdl();
            model.CustomerList = _CustomerService.AllCustomerForDdl();
            model.GS = _GeneralService.GetGeneralSettings();
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateInvoice(Invoice model)
        {
            if (ModelState.IsValid)
            {
                    try
                    {
                        InvoiceHistory IHmodel = new InvoiceHistory();
                        IHmodel.CustomerID = model.CustomerID;
                        IHmodel.LayoutName = model.LayoutName??1.ToString();
                        IHmodel.SpecificNotes = model.SpecificNotes;
                        IHmodel.Status = model.Status;
                        IHmodel.TotalTaxes = model.TotalTaxes;
                        IHmodel.TotalWithoutTax = model.TotalWithoutTax;
                    IHmodel.InvoiceDate =Convert.ToDateTime(model.InvoiceDate);
                    IHmodel.InvoiceDueDate = Convert.ToDateTime(model.InvoiceDueDate);
                    IHmodel.InvoiceNo = model.InvoiceNo;
                    IHmodel.ShareableID = Guid.NewGuid().ToString();
                    IHmodel.TotalWithTaxes = Convert.ToDecimal(model.TotalWithoutTax) + Convert.ToDecimal(model.TotalTaxes);
                    IHmodel.ShowPaymentButton = false;
                    IHmodel.CreatedDate = System.DateTime.UtcNow;
                   
                        int RefInvoiceHistpryID = _InvoiceService.SaveInvoice(IHmodel).ID;

                        foreach (var item in model.SlectedItems)
                        {
                            InvoiceHistoryProducts IHP = new InvoiceHistoryProducts();

                            string[] arr = item.Split('-');
                            int Pid = Convert.ToInt32(arr[0]);
                            int Quantity = Convert.ToInt32(arr[1]);

                            decimal Price = _ProductService.GetProductByID(Pid).ProductUnitPrice;

                            IHP.ProdcutID = Pid;
                            IHP.ProductUnitPrice = Price;
                            IHP.ProdcutUnits = Quantity;
                            IHP.ProdcutSubTotal = Price * Convert.ToDecimal(Quantity);
                            IHP.InvoiceHistoryID = RefInvoiceHistpryID;
                            _InvoiceService.SaveInvoiceProducts(IHP);
                        }

                        return RedirectToAction("InvoiceHistoryList"); // need to do
                    }
                    catch(Exception ex)
                    {
                    TempData["Error"] = "Sorry, Some error occourred";
                        return RedirectToAction("CreateInvoice");
                   
                    }
            }
            else
            {
                model.AllStatusList = StatusList;
                model.LayoutName = model.LayoutName;
                model.AllProducts = _ProductService.AllProductListForDdl();
                model.GS = _GeneralService.GetGeneralSettings();
                return View(model);
            }
            
        }

      
        public IActionResult InvoiceHistoryList(int? page)
        {
            ViewBag.MsgShow = TempData["Error"] ;
            int pageNumber = page ?? 1;
            InvoiceHistoryVM model = new InvoiceHistoryVM();
            model.InvoiceHistoryList =  _InvoiceService.AllInvoices().ToPagedList(pageNumber, PageSize);
            return View(model);
        }

       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatusOfInvoice(int Id, int? page)
        {
            InvoiceHistory IH = _InvoiceService.GetInvoiceByID(Id);
            if(IH.Status.ToLower()=="due")
            {
                IH.Status = "Paid";
            }
           else if (IH.Status.ToLower() == "paid")
            {
                IH.Status = "Due";
            }
            else
            {
                ;
            }
            var dataUpdate = _InvoiceService.UpdateInvoice(IH);
            TempData["Error"] = "Status Changed Successfully.";
            return RedirectToAction("InvoiceHistoryList", new { page = page });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteInvoice(int Id , int? page)
        {
            var data = _InvoiceService.GetInvoiceByID(Id);
            var deltedData = _InvoiceService.DeleteInvoice(data);
            var GetAllProductsFromIH = _InvoiceService.GetInvoiceProductsByInvoiceID(Id);
            foreach(var item in GetAllProductsFromIH)
            {
                var dataProduct = _InvoiceService.DeleteInvoiceProdcuts(item);
            }
            TempData["Error"] = "Data Deleted Successfully.";
            return RedirectToAction("InvoiceHistoryList", new { page= page });
        }

        public IActionResult SingleInvoice(int Id)
        {
            SingleInvoiceVM model = new SingleInvoiceVM();
            model.InvoiceHistory = _InvoiceService.GetInvoiceByID(Id); ;
            model.Products = _InvoiceService.GetInvoiceProductsByInvoiceID(Id);
            model.GS = _GeneralService.GetGeneralSettings();
            return View(model);
        }
        
        

        [AllowAnonymous]
        [Route("/public/{id}")]
        public IActionResult SharedInvoce(string Id)
        {
            SingleInvoiceVM model = new SingleInvoiceVM();
            if(_InvoiceService.GetInvoiceByPublicID(Id)!=null)
            {
                model.InvoiceHistory = _InvoiceService.GetInvoiceByPublicID(Id);
                int InvoiceHisID = model.InvoiceHistory.ID;
                model.Products = _InvoiceService.GetInvoiceProductsByInvoiceID(InvoiceHisID);
                model.GS = _GeneralService.GetGeneralSettings();
                return View(model);
            }
            else
            {
                return RedirectToAction("ResourceNotFound","Error");
            }
           
        }

        [HttpPost]
        public IActionResult EmailInvoiceToClientWithsharedLink(int Id, int? page)
        {
            var InvoiceData = _InvoiceService.GetInvoiceByID(Id);
            int CustomerID = InvoiceData.CustomerID;
            var CustomerData = _CustomerService.GetCustomerByID(CustomerID);
           bool result= SendNotiEmail(CustomerData.Email,CustomerData.Name,InvoiceData.ShareableID,page);
            if(result)
            {
                TempData["Error"] = "Email Sent Successfully.";
                return RedirectToAction("InvoiceHistoryList");

            }
            else
            {
                TempData["Error"] = "Sorry some error occoured, please check your email settings.";
                return RedirectToAction("InvoiceHistoryList");

            }
        }

        public bool SendNotiEmail(string Tomail, string ToName,string SharableID, int? page)
        {
            try
            {
                var EmailSettingsData = _EmailService.GetEmailSettings();
                var GS = _GeneralService.GetGeneralSettings();

                string BodyOfEmail = "Dear " + ToName + " <br/>";
                BodyOfEmail += "Please find your invoice link below :<br/> <br/>";
                BodyOfEmail += "<a href='"+_HttpContextAccessor.HttpContext.Request.Host + "/public/" + SharableID+ "' target='_blank' >click here to see </a> <br/> <br/> If above link doesn't work please copy the below url and paste it in to your browser. <br/> Link : " +
                    _HttpContextAccessor.HttpContext.Request.Host + " /public/" + SharableID;
                var emailMsg = new MailMessage();
                emailMsg.IsBodyHtml = true;
                emailMsg.Body = BodyOfEmail + "<br/><br/>  Regards <br/> <strong> " + GS.Name + " </strong> <br/>" + GS.Address + "<br/>" + GS.Email + "<br/>" + GS.Website;
                emailMsg.Subject = EmailSettingsData.Subject;

                emailMsg.From = new MailAddress(EmailSettingsData.FromEmail.ToString());
                emailMsg.To.Add(new MailAddress(Tomail));

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = EmailSettingsData.Username.ToString(),  
                        Password = EmailSettingsData.EmailPassword.ToString()  
                    };
                    smtp.Credentials = credential;
                    smtp.Host = EmailSettingsData.Host.ToString();
                    smtp.Port = Convert.ToInt32(EmailSettingsData.PortNo);
                    smtp.EnableSsl = Convert.ToBoolean(EmailSettingsData.EnableSsl);
                    smtp.Send(emailMsg);
                }

                return true; // if email sent successfully

            }
            catch (Exception)
            {
                return false; // if email faills
            }

        }

    }
}