using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Models
{
    public class General
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Logo Url")]
        public string LogoUrl { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Invoice Header")]
        public string InvoiceHeader { get; set; }

        [Display(Name = "Invoice Sub Header")]
        public string InvoiceSubHeader { get; set; }

        [Display(Name = "Invoice Footer")]
        public string InvoiceFooter { get; set; }

        [Display(Name = "Terms & Conditions")]
        public string Terms { get; set; }

        [Display(Name = "Taxes")]
        [Required]
        public string Taxes { get; set; }

        [Display(Name = "Invoice Prefix")]
        public string InvoicePrefix { get; set; }

        [Required]
        [Display(Name = "Page Size")]
        public int PageSize { get; set; }

        [Required]
        [Display(Name = "Default Currency")]
        public int CurrencyID { get; set; }

    }
}
