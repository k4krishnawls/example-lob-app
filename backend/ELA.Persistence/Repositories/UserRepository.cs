using Dapper;
using ELA.Common.DTOs.User;
using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Persistence.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        { }

        public async Task<int> CreateUserAsync(UserDTO newUser)
        {
            var param = newUser;
            var sql = @"
                INSERT INTO dbo.[User](Username, [Name], UserTypeId, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy)
                VALUES(@Username, @Name, @UserType, @CreatedOn, @CreatedBy, @UpdatedOn, @UpdatedBy);

                SELECT scope_identity();
            ";
            using (var conn = GetConnection())
            {
                try
                {
                    return await conn.QuerySingleAsync<int>(sql, param);
                }
                catch (SqlException sqlexc)
                {
                    if (sqlexc.Message.Contains("Violation of UNIQUE KEY constraint 'AK_UserName'"))
                    {
                        throw new UsernameIsNotUniqueException(newUser.Username);
                    }
                    throw;
                }
            }
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            var sql = @"
                SELECT Id, 
                        Username, 
                        [Name], 
                        UserType = UserTypeId,
                        CreatedOn,
                        CreatedBy,
                        UpdatedOn,
                        UpdatedBy
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
                        CreatedOn,
                        CreatedBy,
                        UpdatedOn,
                        UpdatedBy
                FROM dbo.[User]
                WHERE Id = @Id;
            ";
            using (var conn = GetConnection())
            {
                return await conn.QuerySingleOrDefaultAsync<UserDTO>(sql, param);
            }
        }

        public async Task SaveUserAsync(UserDTO user)
        {
            var param = user;
            var sql = @"
                UPDATE dbo.[User]
                SET Username = @Username, 
                    [Name] = @Name,
                    UpdatedBy = @UpdatedBy,
                    UpdatedOn = @UpdatedOn
                WHERE Id = @Id;
            ";
            using (var conn = GetConnection())
            {
                try
                {
                    await conn.ExecuteAsync(sql, param);
                }
                catch (SqlException sqlexc)
                {
                    if (sqlexc.Message.Contains("Violation of UNIQUE KEY constraint 'AK_UserName'"))
                    {
                        throw new UsernameIsNotUniqueException(user.Username);
                    }
                    throw;
                }
            }
        }
    }
}
