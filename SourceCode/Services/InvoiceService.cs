using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SourceCode.Models;

namespace SourceCode.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDBContext _Context ;

        public InvoiceService(AppDBContext _context )
        {
            _Context = _context;
        }

        public IEnumerable<InvoiceHistory> AllInvoices()
        {
           return _Context.InvoiceHistory.AsNoTracking().OrderByDescending(x => x.ID).ToList();
        }

        public InvoiceHistory SaveInvoice(InvoiceHistory Invoice)
        {
            // Invoice Master
            _Context.InvoiceHistory.Add(Invoice);
            _Context.SaveChanges();
            return Invoice;
        }

        public InvoiceHistoryProducts SaveInvoiceProducts(InvoiceHistoryProducts SelectedProducts)
        {
         
            //Invoice related products
            _Context.InvoiceHistoryProducts.Add(SelectedProducts);
            _Context.SaveChanges();
            return SelectedProducts;
        }

        public IEnumerable<InvoiceHistoryProducts> GetInvoiceProductsByInvoiceID(int ID)
        {
            return _Context.InvoiceHistoryProducts.AsNoTracking().Where(x=>x.InvoiceHistoryID==ID).ToList();
        }

        public InvoiceHistory GetInvoiceByID(int ID)
        {
            return _Context.InvoiceHistory.Find(ID);
        }

        public InvoiceHistory GetInvoiceByPublicID(string ID)
        {
            return _Context.InvoiceHistory.Where(x=>x.ShareableID==ID).SingleOrDefault();
        }

        public InvoiceHistory UpdateInvoice(InvoiceHistory Invoice)
        {
            // Invoice Master
            _Context.Entry(Invoice).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _Context.SaveChanges();
            return Invoice;
        }

        public InvoiceHistory DeleteInvoice(InvoiceHistory Invoice)
        {
            // Invoice Master
            _Context.InvoiceHistory.Remove(Invoice);
            _Context.SaveChanges();

            return Invoice;

        }

        public InvoiceHistoryProducts DeleteInvoiceProdcuts(InvoiceHistoryProducts SelectedProduct)
        {
            _Context.InvoiceHistoryProducts.Remove(SelectedProduct);
            _Context.SaveChanges();
            return SelectedProduct;

        }

        public int TotalInvoices()
        {
            return _Context.InvoiceHistory.ToList().Count(); 

        }

        public decimal TotalDues()
        {
            return _Context.InvoiceHistory.Where(x=>x.Status.ToLower()=="due").Sum(x=>x.TotalWithTaxes);

        }

        public decimal TotalPaid()
        {
            return _Context.InvoiceHistory.Where(x => x.Status.ToLower() == "paid").Sum(x => x.TotalWithTaxes);

        }

        public decimal TotalSales()
        {
            return _Context.InvoiceHistory.Sum(x => x.TotalWithTaxes);

        }
    }
}
