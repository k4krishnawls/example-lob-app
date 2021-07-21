using Dapper;
using ELA.Common.DTOs.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class Reviews
    {
        private DatabaseHelper _databaseHelper;

        public Reviews(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public ReviewDTO Add(int customerId, int productId, int orderId, RatingScore rating, ReviewStatus status, DateTime entryDate, string comment)
        {
            using (var conn = _databaseHelper.GetConnection()) {
                var param = new { customerId, productId, orderId, rating, status, entryDate, comment };
                var sql = @"
                    INSERT INTO dbo.Review(CustomerId, ProductId, OrderId, Rating, ReviewStatusId, EntryDate, Comment)
                    VALUES(@CustomerId, @ProductId, @OrderId, @Rating, @Status, @EntryDate, @Comment);
                    SELECT * FROM dbo.Review WHERE Id = scope_identity();
                ";
                return conn.QuerySingle<ReviewDTO>(sql, param);
            }
        }

    }
}
