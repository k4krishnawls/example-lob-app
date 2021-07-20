using Dapper;
using ELA.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class ProductCategories
    {
        private DatabaseHelper _databaseHelper;

        public ProductCategories(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public List<CategoryDTO> Add(List<string> names)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var results = new List<CategoryDTO>();
                foreach (var name in names)
                {
                    var param = new { name };
                    var sql = @"
                    INSERT INTO dbo.Category([Name])
                    VALUES(@Name);
                    SELECT * FROM dbo.Category WHERE Id = scope_identity();
                ";
                    results.Add(conn.QuerySingle<CategoryDTO>(sql, param));
                }
                return results;
            }
        }
    }
}
