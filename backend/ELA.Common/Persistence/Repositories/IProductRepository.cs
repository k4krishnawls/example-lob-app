using ELA.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.Persistence.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductTypeDTO>> GetAllTypesAsync();
    }
}
