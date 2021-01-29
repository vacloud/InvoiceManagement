using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.ViewModels
{
    public class GeneralSettings
    {
          public  General GeneralModel { get; set; }
                  
          public List<SelectListItem> CurrencyList { get; set; }

          public IFormFile LogoImage { get; set; }  

    }
}
