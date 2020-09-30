using ELA.Common.BusinessLogic;
using ELA.Common.DTOs.User;
using ELA.Common.Persistence;
using System;
using System.Collections.Generic;
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

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            return await BusinessQuery(async () => {
                return await _persistence.Users.GetAllAsync();
            });
        }
    }
}
