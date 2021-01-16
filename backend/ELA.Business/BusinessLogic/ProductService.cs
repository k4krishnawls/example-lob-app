using ELA.Common.BusinessLogic;
using ELA.Common.DTOs.Product;
using ELA.Common.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ELA.Business.BusinessLogic
{
    public class ProductService : IProductService
    {
        private IBusinessServiceOperator _busOp;

        public ProductService(IBusinessServiceOperator busOp)
        {
            _busOp = busOp;
        }

        public async Task<List<ProductTypeDTO>> GetAllProductTypesAsync()
        {
            return await _busOp.Query(async (persistence) =>
            { 
                return await persistence.Products.GetAllTypesAsync();
            });
        }
    }
}
