using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.ViewModels
{
    public class Invoice
    {
        public string LayoutName { get; set; }

        public string InvoiceNo { get; set; }

        public string InvoiceDate { get; set; }

        public string InvoiceDueDate { get; set; }

        public General GS { get; set; }

        public List<SelectListItem> CustomerList { get; set; }

        public List<SelectListItem> AllProducts { get; set; }

        public List<SelectListItem> AllStatusList { get; set; }

        [Required(ErrorMessage = "Please Select Status"), Display(Name = "Status")]
        public string Status { get; set; }

        public string SpecificNotes { get; set; }

        public decimal TotalWithoutTax { get; set; }

        public decimal TotalTaxes { get; set; }

        public decimal TotalWithTaxes { get; set; }

        [Required(ErrorMessage = "Please Select Customer"), Display(Name = "Customer")]
        public int CustomerID { get; set; }

        public bool IsTaxApplicable { get; set; }

      public  List<string> SlectedItems { get; set; }

    }
   
}
