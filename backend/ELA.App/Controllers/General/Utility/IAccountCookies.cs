using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELA.App.Controllers.General.Utility
{
    public interface IAccountCookies
    {
        Task SignInAsync(HttpContext httpContext, string cookieName, ClaimsPrincipal principal);
        Task SignOutAsync(HttpContext httpContext, string cookieName);
    }
}
