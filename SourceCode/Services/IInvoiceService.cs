using SourceCode.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Services
{
  public  interface IInvoiceService
    {
        InvoiceHistory SaveInvoice(InvoiceHistory Invoice);

       InvoiceHistoryProducts SaveInvoiceProducts(InvoiceHistoryProducts SelectedProducts);

        IEnumerable<InvoiceHistory> AllInvoices();

        IEnumerable<InvoiceHistoryProducts> GetInvoiceProductsByInvoiceID(int ID);

        InvoiceHistory GetInvoiceByID(int ID);
        InvoiceHistory GetInvoiceByPublicID(string ID);

        InvoiceHistory UpdateInvoice(InvoiceHistory Invoice);

       InvoiceHistory DeleteInvoice(InvoiceHistory Invoice);

        InvoiceHistoryProducts DeleteInvoiceProdcuts(InvoiceHistoryProducts SelectedProduct);

        int TotalInvoices();

        decimal TotalDues();

        decimal TotalPaid();

        decimal TotalSales();

    }
}
