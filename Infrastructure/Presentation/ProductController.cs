using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [Authorize(Roles ="Admin")]
    public class ProductsController(IServicesManger ServicesManger) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductResultDTO>>> GetAllProducts([FromQuery]ProductSpecificationsParameters parameters)
        {
            var products = await ServicesManger.productService.GetAllProductsAsync(parameters);
            return Ok(products);
        }
        [HttpGet("Brands")]
        public async Task<ActionResult<IEnumerable<BrandResultDTO>>> GetAllBrands()
        {
            var brands = await ServicesManger.productService.GetAllBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("Types")]
        public async Task<ActionResult<IEnumerable<TypeResultDTO>>> GetAllTypes()
        {
            var types = await ServicesManger.productService.GetAllTypesAsync();
            return Ok(types);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResultDTO>> GetProduct(int id)
        {
            var product = await ServicesManger.productService.GetProductByIdAsync(id);
            return Ok(product);
        }
    }
}
