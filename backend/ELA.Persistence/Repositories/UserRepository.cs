using Dapper;
using ELA.Common.DTOs.User;
using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        { }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var sql = @"
                SELECT Id, 
                        Username, 
                        [Name], 
                        UserType = UserTypeId,
                        CreatedOn
                FROM dbo.[User];
            ";
            using (var conn = GetConnection())
            {
                return (await conn.QueryAsync<UserDTO>(sql)).ToList();
            }
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var param = new { id };
            var sql = @"
                SELECT Id, 
                        Username, 
                        [Name], 
                        UserType = UserTypeId,
                        CreatedOn
                FROM dbo.[User]
                WHERE Id = @Id;
            ";
            using (var conn = GetConnection())
            {
                return await conn.QuerySingleOrDefaultAsync<UserDTO>(sql, param);
            }
        }
    }
}
