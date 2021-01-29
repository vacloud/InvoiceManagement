using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SourceCode.ViewModels
{
    public class InvoiceHistoryVM
    {
        public IPagedList<InvoiceHistory> InvoiceHistoryList { get; set; }
    }
}
