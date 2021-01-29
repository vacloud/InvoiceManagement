using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.ViewModels
{
    public class EmailSettingsVM
    {
        public EmailSettings EmailSettings { get; set; }

        public List<SelectListItem> TFList { get; set; }
    }
}
