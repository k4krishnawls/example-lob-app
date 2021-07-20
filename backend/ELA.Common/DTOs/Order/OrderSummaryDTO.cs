using ELA.Common.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.DTOs.Order
{
    public class OrderSummaryDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int ItemCount { get; set; }
        public decimal OrderTotal { get; set; }
        public CustomerSummaryDTO Customer { get; set; }
    }
}
