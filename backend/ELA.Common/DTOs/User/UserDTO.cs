using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.DTOs.User
{
    public class UserDTO
    {
        [Obsolete("For serialization only", false)]
        public UserDTO() { }
        public UserDTO(UserCreateDTO createUser, UserType userType, DateTime createdOn, int createdBy)
        {
            // Id will be set by DB
            Username = createUser.Username;
            Name = createUser.Name;
            UserType = userType;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = createdOn;
            UpdatedBy = createdBy;
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }

        public void ApplyUpdate(UserUpdateDTO update, DateTime updatedOn, int updatedBy)
        {
            // Id is not editable
            Username = update.Username;
            Name = update.Name;
            // UserType is not editable
            // CreatedOn is not editable
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }
    }
}
