using ELA.App.Controllers.Frontend;
using ELA.App.Tests.IntegrationTests.DataSetup.Tables;
using ELA.Business;
using ELA.Business.BusinessLogic;
using ELA.Common.DTOs.Product;
using ELA.Common.DTOs.Review;
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
    public class ReviewsControllerTests : IntegrationTestsBase
    {
        private ReviewsController _controller;
        private List<CategoryDTO> _productCategories;
        private List<ProductDTO> _products;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.ClearDatabase();

            _productCategories = Database.ProductCategories.Add(new List<string>() {
                "Category #1"
            });
            _products = new List<ProductDTO>(){
                Database.Products.Add(_productCategories[0].Id, "P1", 1.1M, 1.2M, 1.3M, 4, "P1-T", "P1-I", "P1-Desc")
            };
        }

        [SetUp]
        public void BeforeEachTest()
        {
            var persistence = new DapperPersistence(Database.GetConnectionSettings());
            var busOp = new BusinessServiceOperatorWithRetry(persistence);
            var service = new InteractiveUserQueryService(busOp);
            _controller = new ReviewsController(service)
            {
                ControllerContext = GetControllerContextForFrontEnd()
            };
        }

        [TestCase(ReviewStatus.Accepted, TestName = "GetPendingReviewsAsync_AcceptedReview_IsNotIncluded")]
        [TestCase(ReviewStatus.Rejected, TestName = "GetPendingReviewsAsync_RejectedReview_IsNotIncluded")]
        public async Task GetPendingReviewsAsync_NonPendingCases(ReviewStatus status)
        {
            var customer = Database.Customers.Add("Customer #1");
            var order = Database.Orders.AddOrder(
                OrderBuilder.NewOrder("ABC123", DateTime.UtcNow, customer.Id)
                    .AddLine(1, _products[0])
                    .RawOrder
            );
            var review = Database.Reviews.Add(customer.Id, _products[0].Id, order.Id, RatingScore.Five, status, DateTime.UtcNow, "This is the comment");

            var result = await _controller.GetPendingReviewsAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<ReviewSummaryDTO>>(result);
            resultList.Select(r => r.Id).Should().NotContain(review.Id);
        }

        [Test]
        public async Task GetPendingReviewsAsync_PendingCases_ReturnsPendingReviews()
        {
            var customer = Database.Customers.Add("Customer #1");
            var order = Database.Orders.AddOrder(
                OrderBuilder.NewOrder("ABC123", DateTime.UtcNow, customer.Id)
                    .AddLine(1, _products[0])
                    .RawOrder
            );
            var reviews = new List<ReviewDTO>(){
                Database.Reviews.Add(customer.Id, _products[0].Id, order.Id, RatingScore.Five, ReviewStatus.Pending, DateTime.UtcNow, "This is the comment"),
                Database.Reviews.Add(customer.Id, _products[0].Id, order.Id, RatingScore.Five, ReviewStatus.Pending, DateTime.UtcNow, "This is the comment")
            };

            var result = await _controller.GetPendingReviewsAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<ReviewSummaryDTO>>(result);
            var expectedIds = reviews.Select(r => r.Id).ToList();
            resultList.Select(r => r.Id).Should().Contain(expectedIds);
            resultList.Where(r => expectedIds.Contains(r.Id))
                .Select(r => r.Customer.Id)
                .Should().Contain(reviews.Select(r => r.CustomerId));
        }

    }
}
