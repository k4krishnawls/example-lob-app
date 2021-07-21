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

        [TestCase(OrderStatus.Ordered, true, TestName = "GetPendingOrdersAsync_OrderedStatus_IsIncludedInPending")]
        [TestCase(OrderStatus.Processing, true, TestName = "GetPendingOrdersAsync_ProcessingStatus_IsIncludedInPending")]
        [TestCase(OrderStatus.Delivering, true, TestName = "GetPendingOrdersAsync_DeliveringStatus_IsIncludedInPending")]
        [TestCase(OrderStatus.Delivered, false, TestName = "GetPendingOrdersAsync_DeliveredStatus_IsExcludedFromPending")]
        [TestCase(OrderStatus.Cancelled, false, TestName = "GetPendingOrdersAsync_CancelledStatus_IsExcludedFromPending")]
        public async Task GetPendingOrdersAsync_OrderInGivenStatus_MayOrMayNotBeIncluded(OrderStatus status, bool shouldBeIncluded)
        {
            var customer = Database.Customers.Add("Customer #1");
            var rawOrder = OrderBuilder.NewOrder("C1-0", DateTime.UtcNow, customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(status);
            var order = Database.Orders.AddOrder(rawOrder.RawOrder);

            var result = await _controller.GetPendingOrdersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<OrderSummaryDTO>>(result);
            if (shouldBeIncluded)
            {
                resultList.Count.Should().BeGreaterOrEqualTo(1);
                resultList.Select(r => r.Id).Should().Contain(order.Id);
            }
            else
            {
                resultList.Select(r => r.Id).Should().NotContain(order.Id);
            }
        }

        [Test]
        public async Task GetPendingOrdersAsync_MultiplePendingOrders_WillAllBeIncluded()
        {
            var customer = Database.Customers.Add("Customer #2");
            var rawOrders = new List<OrderDTO>() {
                OrderBuilder.NewOrder("C2-0", DateTime.UtcNow, customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Ordered)
                    .RawOrder,
                OrderBuilder.NewOrder("C2-1", DateTime.UtcNow, customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Processing)
                    .RawOrder,
                OrderBuilder.NewOrder("C2-2", DateTime.UtcNow, customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Delivering)
                    .RawOrder
            };
            var orders = Database.Orders.AddOrders(rawOrders);

            var result = await _controller.GetPendingOrdersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<OrderSummaryDTO>>(result);
            resultList.Count.Should().BeGreaterOrEqualTo(3);
            var forThisTest = resultList.Where(r => r.Customer.Id == customer.Id).ToList();
            forThisTest.Should().HaveCount(3);
            forThisTest.Select(f => f.Id).Should().BeEquivalentTo(orders.Select(o => o.Id));
        }

        [Test]
        public async Task GetNewCustomerOrdersAsync_NewCustomer_ReturnsNewOrder()
        {
            var customer = Database.Customers.Add("Customer #3");
            // we're assuming orders will be injected to DB in the order in this list
            var rawOrders = new List<OrderDTO>() {
                OrderBuilder.NewOrder("C3-0", DateTime.UtcNow.AddDays(-2), customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Ordered)
                    .RawOrder
            };
            var orders = Database.Orders.AddOrders(rawOrders);

            var result = await _controller.GetNewCustomerOrdersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<OrderSummaryDTO>>(result);
            resultList.Count.Should().BeGreaterOrEqualTo(1);
            var newCustomerOrders = resultList.Select(r => (OrderId: r.Id, CustomerId: r.Customer.Id)).ToList();
            newCustomerOrders.Should().Contain((OrderId: orders[0].Id, CustomerId: customer.Id));
        }

        [Test]
        public async Task GetNewCustomerOrdersAsync_NewCustomerWithTwoOrders_OnlyReturnsFirstNewOrder()
        {
            var customer = Database.Customers.Add("Customer #3");
            // we're assuming orders will be injected to DB in the order in this list
            var rawOrders = new List<OrderDTO>() {
                OrderBuilder.NewOrder("C3-0", DateTime.UtcNow.AddDays(-2), customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Ordered)
                    .RawOrder,
                OrderBuilder.NewOrder("C3-1", DateTime.UtcNow.AddDays(-1), customer.Id)
                    .AddLine(1, _products[0])
                    .SetStatus(OrderStatus.Processing)
                    .RawOrder
            };
            var orders = Database.Orders.AddOrders(rawOrders);

            var result = await _controller.GetNewCustomerOrdersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<OrderSummaryDTO>>(result);
            resultList.Count.Should().BeGreaterOrEqualTo(1);
            var newCustomerOrders = resultList.Select(r => (OrderId: r.Id, CustomerId: r.Customer.Id)).ToList();
            newCustomerOrders.Should().Contain((OrderId: orders[0].Id, CustomerId: customer.Id));
            newCustomerOrders.Should().NotContain((OrderId: orders[1].Id, CustomerId: customer.Id));
        }

    }
}