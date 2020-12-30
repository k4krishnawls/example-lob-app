using ELA.Common.Authorization;
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
        Task<UserDTO> GetUserAsync(int id);
        Task<int> CreateUserAsync(UserCreateDTO createUser, IAuthContext authContext);
        Task UpdateUserAsync(UserUpdateDTO update, IAuthContext authContext);
    }
}
