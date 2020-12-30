using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.DTOs.User
{
    public class UsernameIsNotUniqueException : UserPresentableException
    {
        public UsernameIsNotUniqueException(string username)
            : base($"Username '{username}' is already in use")
        {
            Data.Add("Username", username);
        }

        public UsernameIsNotUniqueException(string username, Exception innerException)
            : base($"Username '{username}' is already in use", innerException)
        {
            Data.Add("Username", username);
        }
    }
}
