using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ELA.App.StartupConfiguration
{
    public static class SecurityConstants
    {
        public const string CookieAuthScheme = "Cookie";

        public const string Claim_SessionId = "sid";
        public const string Claim_UserId = "uid";
        public const string Claim_UserName = "name";

        public const string Policy_InteractiveUserAccess = "AuthPolicy:InteractiveUserAccess";

        public const string CORS_AllowAny = "CORSPolicy:AllowAny";
    }
}
