using Dapper;
using ELA.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
    public class ProductTypes
    {
        private DatabaseHelper _databaseHelper;

        public ProductTypes(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public ProductTypeDTO Add(string displayName, DateTime? updatedOn = null, int? updatedBy = DatabaseHelper.ADMINUSERID)
        {
            updatedOn = updatedOn ?? DateTime.UtcNow;

            using (var conn = _databaseHelper.GetConnection())
            {
                var param = new { displayName, updatedOn, updatedBy };
                var sql = @"
                    INSERT INTO dbo.ProductType(DisplayName, UpdatedOn, UpdatedBy)
                    VALUES(@DisplayName, @UpdatedOn, @UpdatedBy);
                    SELECT * FROM dbo.ProductType WHERE Id = scope_identity();
                ";
                return conn.QuerySingle<ProductTypeDTO>(sql, param);
            }
        }
    }
}
