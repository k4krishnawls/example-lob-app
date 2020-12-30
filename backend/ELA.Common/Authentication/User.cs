using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.Authentication
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool MustResetPassword { get; set; }
    }
}
