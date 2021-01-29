using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SourceCode.Controllers
{
    // Error Controller
    public class ErrorController : Controller
    {
        public IActionResult ResourceNotFound()
        {
            Response.StatusCode = 404; 
            return View();
        }
    }
}