using Dapper;
using ELA.Common.Persistence;
using ELA.Common.Persistence.Repositories;
using ELA.Persistence.Conversions;
using ELA.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ELA.Persistence
{
    public class DapperPersistence : IPersistence
    {
        private DatabaseConnectionSettings _settings;

        public DapperPersistence(DatabaseConnectionSettings settings)
        {
            _settings = settings;

            PatchDapper();

            Customers = new CustomerRepository(_settings.Database);
        }

        public ICustomerRepository Customers { get; }

        public static void PatchDapper()
        {
            SqlMapper.AddTypeHandler(new DateTimeHandler());
        }

    }
}
