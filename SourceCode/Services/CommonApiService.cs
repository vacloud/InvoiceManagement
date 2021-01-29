using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SourceCode.Models;

namespace SourceCode.Services
{
    // Service for APIs
    public class CommonApiService : ICommonApiService
    {
        private readonly IProductService _ProductService;

        public ICustomerService _CustomerService { get; }

        public CommonApiService(IProductService _productService , ICustomerService _customerService)
        {
            _ProductService = _productService;
            _CustomerService = _customerService;
        }
        public Product SingleProductDetails(int Id)
        {
            return _ProductService.GetProductByID(Id);
        }

        public Customer SingleCustomerDetails(int Id)
        {
            return _CustomerService.GetCustomerByID(Id);
        }

      
    }
}
