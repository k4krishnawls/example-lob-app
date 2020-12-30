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
        Task<UserDTO> GetByIdAsync(int id);
        Task<int> CreateUserAsync(UserDTO newUser);
        Task SaveUserAsync(UserDTO user);
    }
}
