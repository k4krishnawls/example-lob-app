using ELA.App.Security;
using ELA.Common.BusinessLogic;
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
            var customers = await _userService.GetAllUsersAsync();
            return Ok(customers);
        }
    }
}
