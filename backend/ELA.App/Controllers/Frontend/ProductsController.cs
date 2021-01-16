using ELA.App.Security;
using ELA.Common.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELA.App.Controllers.Frontend
{
    [Route("api/fe/products")]
    [Authorize(Policy = Policies.InteractiveUserAccess)]
    public class ProductsController : Controller
    {
        private IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypesAsync()
        {
            var products = await _productService.GetAllProductTypesAsync();
            return Ok(products);
        }
    }
}
