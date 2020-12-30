using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ELA.Common.DTOs.User
{
    public class UserCreateDTO
    {
        [Obsolete("For serialization only", false)]
        public UserCreateDTO() { }

        public UserCreateDTO(string username, string name)
        {
            Username = username;
            Name = name;
        }

        [Required]
        [EmailAddress]
        [MaxLength(80)]
        public string Username { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(80)]
        public string Name { get; set; }
    }
}
