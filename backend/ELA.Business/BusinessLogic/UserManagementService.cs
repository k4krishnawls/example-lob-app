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
    public class UserManagementService : IUserManagementService
    {
        private IBusinessServiceOperator _busOp;

        public UserManagementService(IBusinessServiceOperator busOp)
        {
            _busOp = busOp;
        }

        public async Task<int> CreateUserAsync(UserCreateDTO createUser, IAuthContext authContext)
        {
            return await _busOp.Operation(async (persistence) =>
            {
                var newUser = new UserDTO(createUser, UserType.InteractiveUser, DateTime.UtcNow, authContext.UserId);
                return await persistence.Users.CreateUserAsync(newUser);
            });
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await _busOp.Query(async (persistence) =>
            {
                return await persistence.Users.GetAllAsync();
            });
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            return await _busOp.Query(async (persistence) =>
            {
                return await persistence.Users.GetByIdAsync(id);
            });
        }

        public async Task UpdateUserAsync(UserUpdateDTO update, IAuthContext authContext)
        {
            await _busOp.Operation(async (persistence) =>
            {
                var user = await persistence.Users.GetByIdAsync(update.Id);
                if (user == null)
                {
                    throw new NotFoundException("User", update.Id);
                }
                user.ApplyUpdate(update, DateTime.UtcNow, authContext.UserId);
                await persistence.Users.SaveUserAsync(user);
            });
        }
    }
}
