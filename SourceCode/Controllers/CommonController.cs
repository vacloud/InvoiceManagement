using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SourceCode.Services;
using SourceCode.ViewModels;

namespace SourceCode.Controllers
{
   //// [Produces("application/json")]
   // APIs Controller
    public class CommonController : Controller
    {
        private readonly ICommonApiService _CommonApiService;

        public CommonController(ICommonApiService _commonApiService )
        {
            _CommonApiService = _commonApiService;
        }
        
        [HttpGet]
        public IActionResult SingleCustomer(int id)
        {
            var data = _CommonApiService.SingleCustomerDetails(id);

            SingleCustomer SC = new SingleCustomer();
            if (data == null)
            {
                return NotFound();
            }
            SC.Customer = data;
            SC.Message = "Data Found";
            SC.ResponseCode = 1;
            return Json(SC);
        }
        [HttpGet]
        public IActionResult SingleProduct(int id)
        {
            var data =  _CommonApiService.SingleProductDetails(id);

            SingleProduct SP = new SingleProduct();
            if (data == null)
            {
                return NotFound();
            }
            SP.Product = data;
            SP.Message = "Data Found";
            SP.ResponseCode = 1;
            return Json(SP);
        }
    }
}