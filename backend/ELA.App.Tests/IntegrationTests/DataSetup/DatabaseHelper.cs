using Dapper;
using ELA.App.Tests.IntegrationTests.DataSetup.Tables;
using ELA.Persistence;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ELA.App.Tests.IntegrationTests.DataSetup
{
    public class DatabaseHelper
    {
        public const int ADMINUSERID = -1;
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
            DapperPersistence.PatchDapper();

            Customers = new Customers(this);
            Orders = new Orders(this);
            Products = new Products(this);
            ProductCategories = new ProductCategories(this);
            ProductTypes = new ProductTypes(this);
            Users = new Users(this);
        }

        public Customers Customers { get; }
        public CustomerGroups CustomerGroups { get; }
        public Orders Orders { get; }
        public Products Products { get; }
        public ProductTypes ProductTypes { get; }
        public ProductCategories ProductCategories { get; }
        public Users Users { get; }




        public string GetConnectionString()
        {
            return _connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }

        internal DatabaseConnectionSettings GetConnectionSettings()
        {
            return new DatabaseConnectionSettings
            {
                Database = _connectionString
            };
        }

        public void ClearDatabase()
        {
            TestContext.Out.WriteLine("ClearDatabase: Resetting database contents");

            var cleanupScript = File.ReadAllText("./IntegrationTests/DataSetup/0001-ResetDatabase.sql");

            using (var conn = GetConnection())
            {
                conn.Execute(cleanupScript, commandType: System.Data.CommandType.Text);
            }

            TestContext.Out.WriteLine("ClearDatabase: Database reset successfully");
        }
    }
}
