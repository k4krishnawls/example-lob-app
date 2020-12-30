using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.Authentication
{
    public class UserWithCreds
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool MustResetPassword { get; set; }
        public string PasswordHash { get; set; }

        // I would suggest actually tracking this in a separate 1:many table for longer term audit + visibility processes
        public string PasswordResetToken { get; set; }
        public DateTime PasswordResetTokenGoodThrough { get; set; }
    }
}
