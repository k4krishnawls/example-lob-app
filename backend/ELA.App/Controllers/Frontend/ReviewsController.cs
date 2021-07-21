using ELA.App.Security;
using ELA.Business.BusinessLogic;
using ELA.Common.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELA.App.Controllers.Frontend
{
    [Route("api/fe/reviews")]
    [Authorize(Policy = Policies.InteractiveUserAccess)]
    public class ReviewsController : Controller
    {
        private IInteractiveUserQueryService _userQueries;

        public ReviewsController(IInteractiveUserQueryService userQueries)
        {
            _userQueries = userQueries;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingReviewsAsync()
        {
            var reviews = await _userQueries.GetPendingReviewsAsync();
            return Ok(reviews);
        }
    }
}
