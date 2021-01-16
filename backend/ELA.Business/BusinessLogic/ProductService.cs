using ELA.Common.BusinessLogic;
using ELA.Common.DTOs.Product;
using ELA.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Business.BusinessLogic
{
    public class ProductService : BusinessServiceBase, IProductService
    {
        private IPersistence _persistence;

        public ProductService(IPersistence persistence)
        {
            _persistence = persistence;
        }

        public async Task<List<ProductTypeDTO>> GetAllProductTypesAsync()
        {
            return await BusinessQuery(async () => {
                return await _persistence.Products.GetAllTypesAsync();
            });
        }
    }
}
