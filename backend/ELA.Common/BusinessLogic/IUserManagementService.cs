using ELA.Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.BusinessLogic
{
    public interface IUserManagementService
    {
        Task<List<UserDTO>> GetAllUsersAsync();
    }
}
