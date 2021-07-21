using ELA.Common.DTOs.Customer;
using ELA.Common.DTOs.Order;
using ELA.Common.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.BusinessLogic
{
    public interface IInteractiveUserQueryService
    {
        Task<List<CustomerDTO>> GetAllCustomersAsync();
        Task<List<OrderSummaryDTO>> GetPendingOrdersAsync();
        Task<List<ReviewSummaryDTO>> GetPendingReviewsAsync();
    }
}
