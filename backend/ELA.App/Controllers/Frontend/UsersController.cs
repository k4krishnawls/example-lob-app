using ELA.App.Controllers.Frontend.Models;
using ELA.App.Security;
using ELA.Common.BusinessLogic;
using ELA.Common.DTOs;
using ELA.Common.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELA.App.Controllers.Frontend
{
    [Route("api/fe/users")]
    [Authorize(Policy = Policies.InteractiveUserAccess)]
    public class UsersController : Controller
    {
        private IUserManagementService _userService;

        public UsersController(IUserManagementService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateDTO newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BadRequestResponseModel(ModelState));
            }

            try
            {
                var newId = await _userService.CreateUserAsync(newUser, UserAuthContext.From(User));
                return Ok(new CreateUserResponseModel(newId));
            }
            catch (UsernameIsNotUniqueException e)
            {
                return BadRequest(new BadRequestResponseModel(e.UserMessage));
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] UserUpdateDTO update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BadRequestResponseModel(ModelState));
            }

            try
            {
                update.Id = id;
                await _userService.UpdateUserAsync(update, UserAuthContext.From(User));
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (UsernameIsNotUniqueException e)
            {
                return BadRequest(new BadRequestResponseModel(e.UserMessage));
            }
        }
    }
}
