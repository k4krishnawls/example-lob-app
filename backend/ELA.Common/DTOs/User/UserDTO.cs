using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.DTOs.User
{
    public class UserDTO
    {
        public UserDTO() { }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
