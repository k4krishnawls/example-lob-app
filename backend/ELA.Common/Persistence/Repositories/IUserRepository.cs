using ELA.Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAllAsync();
    }
}
