using ELA.Common.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Common.BusinessLogic
{
    public interface IProductService
    {
        Task<List<ProductTypeDTO>> GetAllProductTypesAsync();
    }
}
