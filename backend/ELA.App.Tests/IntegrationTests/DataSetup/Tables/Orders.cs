using Dapper;
using ELA.Common.DTOs.Order;
using ELA.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class Orders
    {
        private DatabaseHelper _databaseHelper;

        public Orders(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public OrderDTO AddOrder(OrderDTO rawOrder)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                // order
                var param = new
                {
                    rawOrder.Reference,
                    rawOrder.OrderDate,
                    rawOrder.CustomerId,
                    rawOrder.SubTotal,
                    rawOrder.DeliveryFeeTotal,
                    rawOrder.TaxRate,
                    rawOrder.SalesTax,
                    rawOrder.Total,
                    rawOrder.OrderStatus,
                    rawOrder.Returned,
                    rawOrder.ShipToName,
                    rawOrder.AddressLine1,
                    rawOrder.AddressLine2,
                    rawOrder.City,
                    rawOrder.StateAbbr,
                    rawOrder.ZipCode
                };
                var sql = @"
                    INSERT INTO dbo.[Order](Reference, OrderDate, CustomerId, SubTotal, DeliveryFeeTotal, TaxRate, SalesTax, Total, OrderStatusId, Returned, ShipToName, AddressLine1, AddressLine2, City, StateAbbr, ZipCode)
                    VALUES(@Reference, @OrderDate, @CustomerId, @SubTotal, @DeliveryFeeTotal, @TaxRate, @SalesTax, @Total, @OrderStatus, @Returned, @ShipToName, @AddressLine1, @AddressLine2, @City, @StateAbbr, @ZipCode);
                    SELECT *, OrderStatus = OrderStatusId FROM dbo.[Order] WHERE Id = scope_identity();
                ";
                var order = conn.QuerySingle<OrderDTO>(sql, param);

                // lines
                foreach (var line in rawOrder.OrderLines)
                {
                    var lineParam = new { order.Id, line.ProductId, line.Quantity };
                    var lineSQL = @"
                        INSERT INTO dbo.OrderLine(OrderId, ProductId, Quantity)
                        VALUES(@Id, @ProductId, @Quantity);
                        SELECT * FROM OrderLine WHERE Id = scope_identity();
                    ";
                    order.OrderLines.Add(conn.QuerySingle<OrderLineDTO>(lineSQL, lineParam));
                }

                return order;
            }

        }

        public List<OrderDTO> AddOrders(IEnumerable<OrderDTO> rawOrders)
        {
            return rawOrders.Select(ro => AddOrder(ro)).ToList();
        }
    }

    public class OrderBuilder
    {
        public OrderDTO RawOrder { get; } = new OrderDTO();

        public static OrderBuilder NewOrder(string reference, DateTime orderDate, int customerId)
        {
            var builder = new OrderBuilder();
            builder.RawOrder.Reference = reference;
            builder.RawOrder.OrderDate = orderDate;
            builder.RawOrder.OrderStatus = OrderStatus.Delivered;
            builder.RawOrder.CustomerId = customerId;
            builder.RawOrder.SubTotal = 0;
            builder.UpdateMath();
            builder.RawOrder.ShipToName = "Ship To Name";
            builder.RawOrder.AddressLine1 = "Address 1";
            builder.RawOrder.City = "City Name";
            builder.RawOrder.StateAbbr = "PR";
            builder.RawOrder.ZipCode = "55555-5555";
            return builder;
        }

        public OrderBuilder SetStatus(OrderStatus orderStatus)
        {
            RawOrder.OrderStatus = orderStatus;
            return this;
        }

        private void UpdateMath()
        {
            RawOrder.SalesTax = RawOrder.TaxRate * RawOrder.SubTotal;
            RawOrder.Total = RawOrder.SubTotal + RawOrder.SalesTax + RawOrder.DeliveryFeeTotal;
        }

        public OrderBuilder AddLine(int quantity, ProductDTO product)
        {
            RawOrder.OrderLines.Add(new OrderLineDTO(new CreateOrderLine(product.Id, quantity)));
            RawOrder.SubTotal += Math.Round(product.Price * quantity, 2);
            UpdateMath();
            return this;
        }
    }
}
