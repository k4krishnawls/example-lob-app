using Dapper;
using ELA.Common.DTOs.Customer;
using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Persistence.Repositories
{
    public class CustomerRepository : BaseRepository, ICustomerRepository
    {
        public CustomerRepository(string connectionString) : base(connectionString)
        { }

        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            var sql = @"
                SELECT C.Id, 
                        C.[Name],
                        C.Email,
                        C.Avatar,
                        C.NewsletterOptIn,
                        CCGX.CustomerGroupId
                FROM dbo.Customer C
                    LEFT JOIN dbo.Customer_CustomerGroup_Xref CCGX ON CCGX.CustomerId = C.Id;
            ";
            using (var conn = GetConnection())
            {
                var customers = new Dictionary<int, CustomerDTO>();
                var rawResults = (await conn.QueryAsync<CustomerDTO, int?, CustomerDTO>(
                    sql,
                    (c, g) =>
                    {
                        if (!customers.ContainsKey(c.Id))
                        {
                            c.CustomerGroupIds = new List<int>();
                            customers.Add(c.Id, c);
                        }
                        else
                        {
                            c = customers[c.Id];
                        }
                        if (g.HasValue)
                        {
                            c.CustomerGroupIds.Add(g.Value);
                        }
                        return c;
                    },
                    splitOn: "CustomerGroupId"
                )).ToList();
                return customers.Values.ToList();
            }
        }
    }
}
