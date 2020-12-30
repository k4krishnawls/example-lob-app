using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.Authorization
{
    public interface IAuthContext
    {
        public int UserId { get; }
    }
}
