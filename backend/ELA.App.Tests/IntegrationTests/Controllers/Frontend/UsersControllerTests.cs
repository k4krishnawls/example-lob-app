using Dapper;
using ELA.App.Controllers.Frontend;
using ELA.App.Controllers.Frontend.Models;
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
        public const int NONEXISTENT_USER_ID = -1000;
        private UsersController _controller;
        private UserDTO _exampleUser;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Database.ClearDatabase();
            _exampleUser = Database.Users.Add("basic unit test user");
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
            var user = _exampleUser;

            var result = await _controller.GetUsersAsync();

            var resultList = AssertResponseIs<OkObjectResult, List<UserDTO>>(result);
            resultList.Should().HaveCountGreaterOrEqualTo(2)
                .And.Contain(u => u.Id == -1 && u.UserType == UserType.SystemUser)
                .And.ContainEquivalentOf(user);
            // vs
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<List<UserDTO>>()
                .Which.Should()
                    .HaveCountGreaterOrEqualTo(2)
                    .And.Contain(u => u.Id == -1 && u.UserType == UserType.SystemUser)
                    .And.ContainEquivalentOf(user);
        }

        [Test]
        public async Task GetUserAsync_ValidId_ReturnsIdentifiedUsers()
        {
            var user = _exampleUser;

            var result = await _controller.GetUserAsync(user.Id);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(user);
        }

        [Test]
        public async Task GetUserAsync_NonExistentId_ReturnsNotFound()
        {
            // no setup

            var result = await _controller.GetUserAsync(NONEXISTENT_USER_ID);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task CreateAsync_ValidParams_CreatesUserAndReturnsId()
        {
            var newUser = new UserCreateDTO("new username", "new user");

            var result = await _controller.CreateAsync(newUser);

            //var newId = result.Should().BeOfType<OkObjectResult>()
            //    .Which.Value.Should().BeOfType<CreateUserResponseModel>()
            //    .Which.Id;
            var newId = AssertResponseIs<OkObjectResult, CreateUserResponseModel>(result).Id;
            using (var conn = Database.GetConnection()) {
                var user = conn.QuerySingle<UserDTO>("SELECT *, UserType = UserTypeId FROM [User] WHERE Id = @newId", new { newId });
                user.Username.Should().Be(newUser.Username);
                user.Name.Should().Be(newUser.Name);
            }
        }

        [Test]
        public async Task CreateAsync_InvalidParams_ReturnsBadRequest()
        {
            var newUser = new UserCreateDTO("new username", "new user");
            // this test does not test the built-in validation of annotations, it tests that we do the expected thing when an error was added to modelstate
            _controller.ModelState.AddModelError("Username", "You did something wrong with this field!!!");

            var result = await _controller.CreateAsync(newUser);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task CreateAsync_DuplicateUsername_ReturnsBadRequest()
        {
            var newUser = new UserCreateDTO(_exampleUser.Username, "new user");

            var result = await _controller.CreateAsync(newUser);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public async Task UpdateAsync_ValidParams_UpdatesUserAndReturnsId()
        {
            var existingUser = Database.Users.Add("existing user for edit", "existing user");
            var update = new UserUpdateDTO("existing user for edit - new name", "existing user - new name");

            var result = await _controller.UpdateAsync(existingUser.Id, update);

            result.Should().BeOfType<OkResult>();
            using (var conn = Database.GetConnection())
            {
                var user = conn.QuerySingle<UserDTO>("SELECT *, UserType = UserTypeId FROM [User] WHERE Id = @Id", existingUser);
                user.Username.Should().Be(update.Username);
                user.Name.Should().Be(update.Name);
                // unchanged
                user.CreatedOn.Should().Be(existingUser.CreatedOn);
            }
        }

        [Test]
        public async Task UpdateAsync_InvalidParams_ReturnsBadRequest()
        {
            var existingUser = Database.Users.Add("existing user for edit 2", "existing user");
            var update = new UserUpdateDTO("existing user for edit - new name", "existing user - new name");
            // this test does not test the built-in validation of annotations, it tests that we do the expected thing when an error was added to modelstate
            _controller.ModelState.AddModelError("Username", "You did something wrong with this field!!!");

            var result = await _controller.UpdateAsync(existingUser.Id, update);

            result.Should().BeOfType<BadRequestObjectResult>();
            using (var conn = Database.GetConnection())
            {
                var user = conn.QuerySingle<UserDTO>("SELECT *, UserType = UserTypeId FROM [User] WHERE Id = @Id", existingUser);
                // unchanged
                user.Should().BeEquivalentTo(existingUser);
            }
        }

        [Test]
        public async Task UpdateAsync_InvalidId_ReturnsBadRequest()
        {
            var update = new UserUpdateDTO("existing user for edit - new name", "existing user - new name");

            var result = await _controller.UpdateAsync(NONEXISTENT_USER_ID, update);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task UpdateAsync_DuplicateUsername_ReturnsBadRequest()
        {
            var existingUser = Database.Users.Add("existing user for edit 3", "existing user");
            var update = new UserUpdateDTO(_exampleUser.Username, "existing user - new name");

            var result = await _controller.UpdateAsync(existingUser.Id, update);

            result.Should().BeOfType<BadRequestObjectResult>();
            using (var conn = Database.GetConnection())
            {
                var user = conn.QuerySingle<UserDTO>("SELECT *, UserType = UserTypeId FROM [User] WHERE Id = @Id", existingUser);
                // unchanged
                user.Should().BeEquivalentTo(existingUser);
            }
        }
    }
}
