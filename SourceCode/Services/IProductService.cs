using Microsoft.AspNetCore.Mvc.Rendering;
using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
    public interface IProductService
    {
        Product GetProductByID(int ID);

        Product DeleteProduct(Product product);

        Product UpdateProduct(Product product);

        Product AddProduct(Product product);

        IEnumerable<Product> AllProductList();

        List<SelectListItem> AllProductListForDdl();

        IEnumerable<Product> Top5ProductList();

        int TotalProducts();

    }
}
