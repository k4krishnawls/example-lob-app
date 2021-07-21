using ELA.Common.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.Persistence.Repositories
{
    public interface IReviewRepository
    {
        Task<List<ReviewSummaryDTO>> GetReviewSummariesByStatusAsync(ReviewStatus pending);
    }
}
