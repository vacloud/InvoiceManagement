using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
    public interface ICommonApiService
    {
        Product SingleProductDetails(int Id);

        Customer SingleCustomerDetails(int Id);
    }
}
