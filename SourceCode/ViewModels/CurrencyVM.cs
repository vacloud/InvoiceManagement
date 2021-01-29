using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SourceCode.ViewModels
{
    public class CurrencyVM
    {
        public Currency Currency { get; set; }

        public IPagedList<Currency> CurrencyList { get; set; }

    }
}
