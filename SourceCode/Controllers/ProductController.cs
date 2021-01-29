using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SourceCode.ViewModels;
using SourceCode.Services;
using X.PagedList;
using SourceCode.Models;
using Microsoft.AspNetCore.Authorization;

namespace SourceCode.Controllers
{
    // Product Management Controller
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly IGeneralService _GeneralService;
        int PageSize = 0;

        public ProductController(IProductService _productService , IGeneralService _generalService)
        {
            _ProductService = _productService;
            _GeneralService = _generalService;
            PageSize = _GeneralService.GetGeneralSettings().PageSize;
        }

        [HttpGet]
        public IActionResult ProductMaster( int? page, int ID)
        {
           
            int pageNumber = page ?? 1;
            ViewBag.MsgShow = TempData["Error"];
            ProductVM model = new ProductVM();

            model.ProductList = _ProductService.AllProductList().ToPagedList(pageNumber, PageSize);
            if (ID != 0)
            {
                var data = _ProductService.GetProductByID(ID);
                Product prod = new Product
                {
                  ID= data.ID,
                  ProductName=data.ProductName,
                  Description=data.Description,
                  IsActive=data.IsActive,
                  ProductUnitPrice=data.ProductUnitPrice
                };
                model.Product = prod;
            }

            return View(model);

        }

        [HttpPost]
        public IActionResult ProductMaster(ProductVM model, int? page)
        {
            int pageNumber = page ?? 1;

            int PageSize = _GeneralService.GetGeneralSettings().PageSize;

            model.ProductList = _ProductService.AllProductList().ToPagedList(pageNumber, PageSize);

            // We used Product.ID on view, so it will send null value to Product.ID
            // We need to convert null to zero so below line of code is used.
            model.Product.ID = Convert.ToInt32(model.Product.ID);
            // we do not use Product.ID on view  (which basically used by update function) 
            // than above line of code is not required.


            // FOR INSERT
            if (model.Product.ID == 0)
            {

                // Need to remove modeal state of ID else it will give model state invalid error
                ModelState.Remove("Product.ID");
                // this is because we used hidden field  Product.ID on view, 
                // if we do not use this on view than above line is not required


                if (ModelState.IsValid)
                {
                    Product prod = new Product
                    {
                        ProductName = model.Product.ProductName,
                        Description = model.Product.Description,
                        IsActive = true,
                        ProductUnitPrice = model.Product.ProductUnitPrice
                    };
                    model.Product = _ProductService.AddProduct(prod);
                    TempData["Error"] = "Details Added Successfully.";
                    return RedirectToAction("ProductMaster", new { page = page });
                }
                else
                {
                    return View(model);
                }
            }
            else
            {
                // FOR UPDATE
                if (ModelState.IsValid)
                {
                    model.Product = _ProductService.UpdateProduct(model.Product);
                    TempData["Error"] = "Details Updated Successfully.";
                    return RedirectToAction("ProductMaster", new { page = page });
                }
                else
                {
                    return View(model);
                }
            }


        }

        [HttpPost]
        public IActionResult ProductMasterDelete(int ID, int? page)
        {
            var data = _ProductService.GetProductByID(ID);
            _ProductService.DeleteProduct(data);
            TempData["Error"] = "Details Deleted Successfully.";
            return RedirectToAction("ProductMaster", new { page = page });

        }

    }
}