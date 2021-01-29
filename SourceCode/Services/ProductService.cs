using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

namespace SourceCode.Services
{
    // Funtions for performing different tasks on Products
    public class ProductService : IProductService
    {
        private readonly AppDBContext _Context;

        public ProductService(AppDBContext _context)
        {
            _Context = _context;
        }

        public Product AddProduct(Product product)
        {
            var data = _Context.Product.Add(product);
            _Context.SaveChanges();
            return product;
        }

      
        public IEnumerable<Product> AllProductList()
        {
            return _Context.Product.AsNoTracking().OrderByDescending(x => x.ID).ToList();
        }

        public List<SelectListItem> AllProductListForDdl()
        {
            return _Context.Product.AsNoTracking().OrderByDescending(x => x.ID).Select(x=> new SelectListItem { Text=(x.ProductName+" ("+x.ProductUnitPrice +")").ToString() ,Value=x.ID.ToString()}).ToList();
        }

        public Product DeleteProduct(Product product)
        {
            _Context.Product.Remove(product);
            _Context.SaveChanges();
            return product;
        }

        public Product GetProductByID(int ID)
        {
            return _Context.Product.Find(ID);
        }

        public Product UpdateProduct(Product product)
        {
            _Context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _Context.SaveChanges();

            return product;
        }

        public int TotalProducts()
        {
            return _Context.Product.ToList().Count() ;
        }

        public IEnumerable<Product> Top5ProductList()
        {
            return _Context.Product.AsNoTracking().OrderByDescending(x => x.ID).ToList().Take(5);
        }
    }
}
