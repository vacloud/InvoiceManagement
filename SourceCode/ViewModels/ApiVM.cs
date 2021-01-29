using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceCode.Models;
using SourceCode.Services;

namespace SourceCode.ViewModels
{
    public class ApiResponse
    {
        public string Message { get; set; }

        public int ResponseCode { get; set; }

    }
    public class SingleProduct : ApiResponse
    {
      public Product Product { get; set; }

    }
    public class SingleCustomer : ApiResponse
    {
        public Customer Customer { get; set; }

    }
}
