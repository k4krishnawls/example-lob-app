using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Invoice
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DeliveryFeeTotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal SalesTax { get; set; }
        public decimal Total { get; set; }
    }
}
