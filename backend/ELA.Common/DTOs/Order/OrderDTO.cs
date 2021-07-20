using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Order
{
    public class OrderDTO
    {
        public OrderDTO()
        {
            OrderLines = new List<OrderLineDTO>();
        }

        public int Id { get; set; }
        public string Reference { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DeliveryFeeTotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal SalesTax { get; set; }
        public decimal Total { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public bool Returned { get; set; }
        public string ShipToName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateAbbr { get; set; }
        public string ZipCode { get; set; }

        public List<OrderLineDTO> OrderLines { get; set; }
    }
}
