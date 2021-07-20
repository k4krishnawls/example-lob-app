using System.Collections.Generic;

namespace ELA.Common.DTOs.Order
{
    public enum OrderStatus
    {
        Ordered = 1,
        Processing = 2,
        Delivering = 3,
        Delivered = 4,
        Cancelled = 5
    }

}