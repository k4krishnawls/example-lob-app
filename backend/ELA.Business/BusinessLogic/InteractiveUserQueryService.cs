using ELA.Common.BusinessLogic;
using ELA.Common.DTOs.Customer;
using ELA.Common.DTOs.Order;
using ELA.Common.DTOs.Review;
using ELA.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Business.BusinessLogic
{
    public class InteractiveUserQueryService : IInteractiveUserQueryService
    {
        private IBusinessServiceOperator _busOp;

        public InteractiveUserQueryService(IBusinessServiceOperator busOp)
        {
            _busOp = busOp;
        }

        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            return await _busOp.Query(async (persistence) =>
            {
                return await persistence.Customers.GetAllAsync();
            });
        }

        public async Task<List<OrderSummaryDTO>> GetPendingOrdersAsync()
        {
            return await _busOp.Query(async (persistence) =>
            {
                return await persistence.Orders.GetPendingOrdersAsync();
            });
        }

        public async Task<List<ReviewSummaryDTO>> GetPendingReviewsAsync()
        {
            return await _busOp.Query(async (persistence) =>
            {
                return await persistence.Reviews.GetReviewSummariesByStatusAsync(ReviewStatus.Pending);
            });
        }
    }
}
