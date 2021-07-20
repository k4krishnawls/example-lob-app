using Dapper;
using ELA.Common.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class Customers
    {
        private DatabaseHelper _databaseHelper;

        public Customers(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public CustomerDTO Add(string name)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var param = new
                {
                    name,
                    Email = name.Replace(" ", "") + "@place.test"
                };
                var sql = @"
                    INSERT INTO dbo.Customer(Name, Email)
                    VALUES(@Name, @Email);
                    SELECT * FROM dbo.Customer WHERE Id = scope_identity();
                ";
                return conn.QuerySingle<CustomerDTO>(sql, param);
            }
        }
    }
}
