using ELA.Common;
using ELA.Common.Authorization;
using ELA.Common.BusinessLogic;
using ELA.Common.DTOs;
using ELA.Common.DTOs.User;
using ELA.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Business.BusinessLogic
{
    public class UserManagementService : BusinessServiceBase, IUserManagementService
    {
        private IPersistence _persistence;

        public UserManagementService(IPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<int> CreateUserAsync(UserCreateDTO createUser, IAuthContext authContext)
        {
            return await BusinessOperation(async () =>
            {
                var newUser = new UserDTO(createUser, UserType.InteractiveUser, DateTime.UtcNow, authContext.UserId);
                return await _persistence.Users.CreateUserAsync(newUser);
            });
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await BusinessQuery(async () =>
            {
                return await _persistence.Users.GetAllAsync();
            });
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            return await BusinessQuery(async () =>
            {
                return await _persistence.Users.GetByIdAsync(id);
            });
        }

        public async Task UpdateUserAsync(UserUpdateDTO update, IAuthContext authContext)
        {
            await BusinessOperation(async () =>
            {
                var user = await _persistence.Users.GetByIdAsync(update.Id);
                if (user == null)
                {
                    throw new NotFoundException("User", update.Id);
                }
                user.ApplyUpdate(update, DateTime.UtcNow, authContext.UserId);
                await _persistence.Users.SaveUserAsync(user);
            });
        }
    }
}
