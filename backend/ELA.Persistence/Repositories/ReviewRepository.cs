using Dapper;
using ELA.Common.DTOs.Customer;
using ELA.Common.DTOs.Review;
using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Persistence.Repositories
{
    public class ReviewRepository : BaseRepository, IReviewRepository
    {
        public ReviewRepository(string connectionString) : base(connectionString)
        { }

        public async Task<List<ReviewSummaryDTO>> GetReviewSummariesByStatusAsync(ReviewStatus status)
        {
            var param = new { status };
            var sql = @"
                SELECT R.Id,
                        R.Rating,
                        R.Comment,
                        C.Id,
                        C.[Name],
                        C.Avatar
                FROM dbo.Review R 
                    INNER JOIN dbo.Customer C ON C.Id = R.CustomerId
                WHERE R.ReviewStatusId = @Status
                GROUP BY R.Id, R.Rating, R.Comment, 
                         C.Id, C.[Name], C.Avatar;
            ";
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<ReviewSummaryDTO, CustomerSummaryDTO, ReviewSummaryDTO>(
                    sql,
                    (r, c) =>
                    {
                        r.Customer = c;
                        return r;
                    },
                    param);
                return result.ToList();
            }
        }
    }
}
