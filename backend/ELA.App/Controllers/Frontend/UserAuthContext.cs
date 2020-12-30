using ELA.App.Security;
using ELA.Common.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELA.App.Controllers.Frontend
{
    public class UserAuthContext : IAuthContext
    {
        public UserAuthContext(int sessionId, int userId, string username)
        {
            SessionId = sessionId;
            UserId = userId;
            Username = username;
        }

        public int SessionId { get; }
        public int UserId { get; }
        public string Username { get; }

        public static UserAuthContext From(ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirst(ClaimNames.UserId).Value);
            var username = user.FindFirst(ClaimNames.UserName).Value;
            var sessionId = int.Parse(user.FindFirst(ClaimNames.SessionId).Value);

            return new UserAuthContext(sessionId, userId, username);
        }
    }
}
