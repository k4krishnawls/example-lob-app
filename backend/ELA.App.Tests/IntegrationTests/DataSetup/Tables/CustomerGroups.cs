using Dapper;
using ELA.Common.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class CustomerGroups
    {
        private DatabaseHelper _databaseHelper;

        public CustomerGroups(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<CustomerGroupDTO> AddGroups(List<string> names)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var results = new List<CustomerGroupDTO>();
                foreach (var name in names)
                {
                    var param = new { name };
                    var sql = @"
                    INSERT INTO dbo.CustomerGroup([Name])
                    VALUES(@Name);
                    SELECT * FROM dbo.CustomerGroup WHERE Id = scope_identity();
                ";
                    results.Add(conn.QuerySingle<CustomerGroupDTO>(sql, param));
                }
                return results;
            }
        }
    }
}
