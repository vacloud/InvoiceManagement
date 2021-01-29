using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace SourceCode.ViewModels
{
    public class CustomerVM
    {
        public Customer Customer { get; set; }

        public IPagedList<Customer> CustomerList { get; set; }
    }
}
