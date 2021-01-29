using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SourceCode.Models;
using SourceCode.Services;
using SourceCode.ViewModels;
using X.PagedList;

namespace SourceCode.Controllers
{
    // Customer Management Controller
    //[Authorize]
    public class CustomerController : Controller
    {
        private readonly IGeneralService _GeneralService;
        private readonly ICustomerService _CustomerService;
        int PageSize = 0;

        public CustomerController(ICustomerService _customerService, IGeneralService _generalService)
        {
            _GeneralService = _generalService;
            _CustomerService = _customerService;
            PageSize = _GeneralService.GetGeneralSettings().PageSize;
        }

        [HttpGet]
        public IActionResult CustomerMaster( int? page, int ID)
        {

            int pageNumber = page ?? 1;
            ViewBag.MsgShow = TempData["Error"] ;
            CustomerVM model = new CustomerVM();

            model.CustomerList = _CustomerService.AllCustomerList().ToPagedList(pageNumber, PageSize);
            if (ID != 0)
            {
                var data = _CustomerService.GetCustomerByID(ID);
                Customer cust = new Customer
                {
                    ID = data.ID,
                    Name = data.Name,
                    Email = data.Email,
                    IsActive = data.IsActive,
                    Address = data.Address,
                    Phone = data.Phone,
                    Skype = data.Skype
                };
                model.Customer = cust;
            }

            return View(model);

        }

        [HttpPost]
        public IActionResult CustomerMaster(CustomerVM model, int? page)
        {
            int pageNumber = page ?? 1;

            int PageSize = _GeneralService.GetGeneralSettings().PageSize;

            model.CustomerList = _CustomerService.AllCustomerList().ToPagedList(pageNumber, PageSize);

            // We used Customer.ID on view, so it will send null value to Customer.ID
            // We need to convert null to zero so below line of code is used.
            model.Customer.ID = Convert.ToInt32(model.Customer.ID);
            // we do not use Customer.ID on view  (which basically used by update function) 
            // than above line of code is not required.


            // FOR INSERT
            if (model.Customer.ID == 0)
            {

                // Need to remove model state of ID else it will give model state invalid error
                ModelState.Remove("Customer.ID");
                // this is because we used hidden field  Customer.ID on view, 
                // if we do not use this on view than above line is not required


                if (ModelState.IsValid)
                {
                    Customer prod = new Customer
                    {
                        Name = model.Customer.Name,
                        Email = model.Customer.Email,
                        IsActive = true,
                        Address = model.Customer.Address,
                        Phone = model.Customer.Phone,
                        Skype = model.Customer.Skype
                      
                    };
                    model.Customer = _CustomerService.AddCustomer(prod);
                    TempData["Error"] = "Details Added Successfully.";
                    return RedirectToAction("CustomerMaster", new { page = page });
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
                    model.Customer = _CustomerService.UpdateCustomer(model.Customer);
                    TempData["Error"] = "Details Updated Successfully.";
                    return RedirectToAction("CustomerMaster", new {  page = page });
                }
                else
                {
                    return View(model);
                }
            }


        }

        [HttpPost]
        public IActionResult CustomerMasterDelete(int ID, int? page)
        {
            var data = _CustomerService.GetCustomerByID(ID);
            _CustomerService.DeleteCustomer(data);
            TempData["Error"] = "Details Deleted Successfully.";
            return RedirectToAction("CustomerMaster", new { page = page });

        }
    }
}