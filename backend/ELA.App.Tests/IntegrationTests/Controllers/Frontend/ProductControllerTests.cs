using ELA.App.Controllers.Frontend;
using ELA.Business.BusinessLogic;
using ELA.Common.DTOs.Product;
using ELA.Persistence;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.Controllers.Frontend
{

    [TestFixture]
    public class ProductControllerTests : IntegrationTestsBase
    {
        private ProductsController _controller;

        [SetUp]
        public void BeforeEachTest()
        {
            Database.ClearDatabase();
            var persistence = new DapperPersistence(Database.GetConnectionSettings());
            var service = new ProductService(persistence);
            _controller = new ProductsController(service)
            {
                ControllerContext = GetControllerContextForFrontEnd()
            };
        }

        [Test]
        public async Task GetTypesAsync_SomeTypes_ReturnsListSuccessfully()
        {
            var expectedTypes = new List<ProductTypeDTO>() {
                Database.ProductTypes.Add("Unit test A"),
                Database.ProductTypes.Add("Unit test B"),
                Database.ProductTypes.Add("Unit test C")
            };

            var response = await _controller.GetProductTypesAsync();

            response.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedTypes);
        }
    }
}