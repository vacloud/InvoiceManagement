using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Models
{
    public class InvoiceHistoryProducts
    {
        public int ID { get; set; }

        public int InvoiceHistoryID {get;set;}

        public int ProdcutID { get; set; }

        public decimal ProductUnitPrice { get; set; }

        public int ProdcutUnits { get; set; }

        public decimal ProdcutSubTotal { get; set; }
        
    }
}
