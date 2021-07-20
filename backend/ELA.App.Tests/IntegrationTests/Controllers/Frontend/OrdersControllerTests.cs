using ELA.App.Controllers.Frontend;
using ELA.App.Tests.IntegrationTests.DataSetup.Tables;
using ELA.Business;
using ELA.Business.BusinessLogic;
using ELA.Common.DTOs.Customer;
using ELA.Common.DTOs.Order;
using ELA.Common.DTOs.Product;
using ELA.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.Controllers.Frontend
{
    [TestFixture]
    [Category("Database-Tests")]
    public class OrdersControllerTests : IntegrationTestsBase
    {
        private OrdersController _controller;
        private List<CategoryDTO> _productCategories;
        private List<ProductDTO> _products;

        // TBD
        public static List<OrderStatus> GetPendingStatuses()
        {
            return new List<OrderStatus>() {
                OrderStatus.Ordered,
                OrderStatus.Processing,
                OrderStatus.Delivering
            };
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.ClearDatabase();

            _productCategories = Database.ProductCategories.Add(new List<string>() {
                "Category #1",
                "Category #C",
                "Category 42"
            });
            _products = new List<ProductDTO>(){
                Database.Products.Add(_productCategories[0].Id, "P1", 1.1M, 1.2M, 1.3M, 4, "P1-T", "P1-I", "P1-Desc"),
                Database.Products.Add(_productCategories[0].Id, "P2", 2.1M, 2.2M, 2.3M, 4, "P2-T", "P2-I", "P2-Desc"),
                Database.Products.Add(_productCategories[0].Id, "P3", 3.1M, 3.2M, 3.3M, 4, "P3-T", "P3-I", "P3-Desc"),
            };
        }

        [SetUp]
        public void BeforeEachTest()
        {
            var persistence = new DapperPersistence(Database.GetConnectionSettings());
            var busOp = new BusinessServiceOperatorWithRetry(persistence);
            var service = new InteractiveUserQueryService(busOp);
            _controller = new OrdersController(service)
            {
                ControllerContext = GetControllerContextForFrontEnd()
            };
        }

        [Test]
        public async Task GetPendingOrdersAsync_NoRecentOrders_ReturnsEmptySet()
        {
            // no setup

            var result = await _controller.GetPendingOrdersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<OrderSummaryDTO>>(result);
            resultList.Should().HaveCount(0);
        }

        [Test]
        public async Task GetPendingOrdersAsync_MixOfPendingAndDeliveredOrders_ReturnsOnlyPendingOrders()
        {
            var customers = new List<CustomerDTO>(){
                Database.Customers.Add("Customer #1"),
                Database.Customers.Add("Customer #2"),
                Database.Customers.Add("Customer #3"),
            };
            var rawOrders = new List<OrderBuilder>() {
                OrderBuilder.NewOrder("C0-0", DateTime.UtcNow, customers[0].Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Ordered),
                OrderBuilder.NewOrder("C1-0", DateTime.UtcNow, customers[1].Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Processing),
                OrderBuilder.NewOrder("C2-0", DateTime.UtcNow, customers[2].Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Delivering),
                OrderBuilder.NewOrder("C0-1", DateTime.UtcNow, customers[0].Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Delivered),
                OrderBuilder.NewOrder("C0-2", DateTime.UtcNow, customers[0].Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Cancelled)
            };
            var orders = Database.Orders.AddOrders(rawOrders.Select(ro => ro.RawOrder));

            var result = await _controller.GetPendingOrdersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<OrderSummaryDTO>>(result);
            resultList.Should().HaveCount(3);
            var pendingIds = orders.Where(o => GetPendingStatuses().Contains(o.OrderStatus)).Select(o => o.Id);
            resultList.Select(r => r.Id).Should().BeEquivalentTo(pendingIds);
        }
    }
}