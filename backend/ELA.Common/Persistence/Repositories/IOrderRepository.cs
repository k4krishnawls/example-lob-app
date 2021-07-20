using ELA.Common.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.Persistence.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderSummaryDTO>> GetPendingOrdersAsync();
    }
}
