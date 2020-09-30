using ELA.App.Controllers.Frontend;
using ELA.Business.BusinessLogic;
using ELA.Common;
using ELA.Common.DTOs.User;
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
    public class UsersControllerTests : IntegrationTestsBase
    {
        private UsersController _controller;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.ClearDatabase();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            var persistence = new DapperPersistence(Database.GetConnectionSettings());
            var service = new UserManagementService(persistence);
            _controller = new UsersController(service)
            {
                ControllerContext = GetControllerContextForFrontEnd()
            };
        }

        [Test]
        public async Task GetUsersAsync_NoParams_ReturnsAllUsers()
        {
            var user = Database.Users.Add("unit test name");

            var result = await _controller.GetUsersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<UserDTO>>(result);
            resultList.Should().HaveCount(2)
                .And.Contain(u => u.Id == -1 && u.UserType == UserType.SystemUser)
                .And.ContainEquivalentOf(user);
        }
    }
}
