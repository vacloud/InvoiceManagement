using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Models
{
    public class Currency
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Symbol { get; set; }

    }
}
