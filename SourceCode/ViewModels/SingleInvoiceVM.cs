using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.ViewModels
{
    public class SingleInvoiceVM
    {
       public InvoiceHistory InvoiceHistory { get; set; }

        public IEnumerable<InvoiceHistoryProducts> Products { get; set; }

        public General GS { get; set; }
    }
}
