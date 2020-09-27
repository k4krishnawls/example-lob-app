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

        public const string CORS_AllowAny = "CORSPolicy:AllowAny";
    }
}
