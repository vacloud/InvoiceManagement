using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Models
{
    public class EmailSettings
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "From Email")]
        public string FromEmail { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email Password")]
        public string EmailPassword { get; set; }

        [Required]
        [Display(Name = "Port No")]
        public int PortNo { get; set; }

        [Required]
        [Display(Name = "Host")]
        public string Host { get; set; }

        [Required]
        [Display(Name = "Enable SSL")]
        public bool EnableSsl { get; set; }

        [Required]
        [Display(Name = "Subject Line")]
        public string Subject { get; set; }

       

       
    }
}
