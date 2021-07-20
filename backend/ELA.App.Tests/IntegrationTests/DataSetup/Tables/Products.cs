using Dapper;
using ELA.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.App.Tests.IntegrationTests.DataSetup.Tables
{
   
    public class Products
    {
        private DatabaseHelper _databaseHelper;

        public Products(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public ProductDTO Add(int categoryId, string name, decimal width, decimal height, decimal price, int stock, string thumbnail, string image, string description)
        {
            using (var conn = _databaseHelper.GetConnection())
            {
                var param = new { categoryId, name, width, height, price, stock, thumbnail, image, description };
                var sql = @"
                    INSERT INTO dbo.Product(CategoryId, [Name], Width, Height, Price, Thumbnail, [Image], [Description], Stock)
                    VALUES(@CategoryId, @Name, @Width, @Height, @Price, @Thumbnail, @Image, @Description, @Stock);
                    SELECT * FROM dbo.Product WHERE Id = scope_identity();
                ";
                return conn.QuerySingle<ProductDTO>(sql, param);
            }
        }
    }
}
