using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELA.App.Controllers.General.Utility
{
    public class AccountCookies : IAccountCookies
    {
        public async Task SignInAsync(HttpContext httpContext, string cookieName, ClaimsPrincipal principal)
        {
            await httpContext.SignInAsync(cookieName, principal);
        }


        public async Task SignOutAsync(HttpContext httpContext, string cookieName)
        {
            await httpContext.SignOutAsync(cookieName);
        }

    }
}
