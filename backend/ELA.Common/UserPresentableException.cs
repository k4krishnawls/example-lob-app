using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common
{
    public class UserPresentableException : Exception
    {
        public UserPresentableException(string userDisplayableMessage)
            : base(userDisplayableMessage)
        {
            UserMessage = userDisplayableMessage;
        }

        public UserPresentableException(string userDisplayableMessage, Exception innerException)
            : base(userDisplayableMessage, innerException)
        {
            UserMessage = userDisplayableMessage;
        }

        public string UserMessage { get; }
    }
}
