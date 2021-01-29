using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required,Display(Name = "Product or Service Name")]
        public string ProductName { get; set; }

        [Required, Display(Name = "Price Per Unit")]
        [Range(0, 999999999.99, ErrorMessage = "Invalid Amount")]
        public decimal ProductUnitPrice { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

    }
}
