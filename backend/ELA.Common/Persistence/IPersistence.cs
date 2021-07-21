using ELA.Common.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ELA.Common.Persistence
{
    public interface IPersistence
    {
        ICustomerRepository Customers { get; }
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IReviewRepository Reviews { get; }
        IOrderRepository Orders { get; }
    }
}
