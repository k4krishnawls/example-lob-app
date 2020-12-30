using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ELA.Common.DTOs.User
{
    public class UserUpdateDTO
    {
        [Obsolete("For serialization only", false)]
        public UserUpdateDTO() { }

        public UserUpdateDTO(string username, string name)
        {
            Username = username;
            Name = name;
        }

        [JsonIgnore]
        public int Id { get; set; }

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
