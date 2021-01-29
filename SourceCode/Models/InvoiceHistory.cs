using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceCode.Models
{
    public class InvoiceHistory
    {
        public int ID { get; set; }

        public int CustomerID { get; set; }

        public string Status { get; set; }

        public string SpecificNotes { get; set; }

        public decimal TotalWithoutTax { get; set; }

        public decimal TotalTaxes { get; set; }

        public decimal TotalWithTaxes { get; set; }

        public string LayoutName { get; set; }

        public string InvoiceNo { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime InvoiceDueDate { get; set; }

        public string ShareableID { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool ShowPaymentButton { get; set; }

    }
}
