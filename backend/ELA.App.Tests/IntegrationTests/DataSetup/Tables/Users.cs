using Dapper;
using ELA.Common;
using ELA.Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class Users
    {
        private DatabaseHelper _databaseHelper;

        public Users(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public UserDTO Add(string username, string name = "unit test", string passwordHash = "whatever", UserType userType = UserType.InteractiveUser, DateTime? createdOn = null)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var sql = @"
                    INSERT INTO dbo.[User](Username, [Name], PasswordHash, UserTypeId, CreatedOn)
                    VALUES(@Username, @Name, @PasswordHash, @UserType, @CreatedOn);
                    SELECT *, UserType = UserTypeId FROM dbo.[User] WHERE Id = scope_identity();
                ";
                var param = new
                {
                    username,
                    name,
                    passwordHash,
                    userType,
                    CreatedOn = createdOn ?? DateTime.UtcNow
                };
                return conn.QuerySingle<UserDTO>(sql, param);
            }
        }
    }
}
