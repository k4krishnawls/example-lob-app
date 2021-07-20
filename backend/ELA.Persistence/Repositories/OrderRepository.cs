using Dapper;
using ELA.Common.DTOs.Customer;
using ELA.Common.DTOs.Order;
using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Persistence.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(string connectionString) : base(connectionString)
        { }

        public async Task<List<OrderSummaryDTO>> GetPendingOrdersAsync()
        {
            var param = new
            {
                Status = new List<OrderStatus>() {
                    OrderStatus.Ordered,
                    OrderStatus.Processing,
                    OrderStatus.Delivering
                }
            };
            var sql = @"
                SELECT O.Id,
                        O.OrderDate,
                        OrderStatus = O.OrderStatusId,
                        ItemCount = SUM(OL.Quantity),
                        OrderTotal = O.Total,
                        C.Id,
                        C.[Name],
                        C.Avatar
                FROM dbo.[Order] O 
                    LEFT JOIN dbo.OrderLine OL ON OL.OrderId = O.Id
                    INNER JOIN dbo.Customer C ON C.Id = O.CustomerId
                WHERE O.OrderStatusId IN @Status
                GROUP BY O.Id, O.OrderDate, O.OrderStatusId, O.Total, 
                         C.Id, C.[Name], C.Avatar;
            ";
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<OrderSummaryDTO,CustomerSummaryDTO, OrderSummaryDTO>(
                    sql, 
                    (o, c) => {
                        o.Customer = c;
                        return o;
                    },
                    param);
                return result.ToList();
            }
        }
    }
}
