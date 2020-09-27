using ELA.App.Security;
using ELA.App.StartupConfiguration;
using ELA.Common.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace ELA.App.Controllers.Frontend
{
    [Route("api/fe/customers")]
    [Authorize(Policy = Policies.InteractiveUserAccess)]
    public class CustomersController : Controller
    {
        private IInteractiveUserQueryService _userQueries;

        public CustomersController(IInteractiveUserQueryService userQueries)
        {
            _userQueries = userQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var customers = await _userQueries.GetAllCustomersAsync();
            return Ok(customers);
        }
    }
}
