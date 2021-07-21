using ELA.Common.DTOs.Order;
using ELA.Common.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.BusinessLogic
{
    public static class PendingStatusExtensions
    {
        public static List<OrderStatus> GetPendingStatuses(this IInteractiveUserQueryService _)
        {
            return new List<OrderStatus>() {
                OrderStatus.Ordered,
                OrderStatus.Processing,
                OrderStatus.Delivering
            };
        }
    }
}
