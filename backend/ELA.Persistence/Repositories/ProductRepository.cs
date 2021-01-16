using Dapper;
using ELA.Common.DTOs.Product;
using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Persistence.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(string connectionString) : base(connectionString)
        { }

        public async Task<List<ProductTypeDTO>> GetAllTypesAsync()
        {
            var sql = @"
                SELECT Id, 
                       DisplayName,
                       UpdatedBy,
                       UpdatedOn
                FROM dbo.ProductType;                
            ";
            using (var conn = GetConnection())
            {
                return (await conn.QueryAsync<ProductTypeDTO>(sql))
                    .ToList();
            }
        }
    }
}
