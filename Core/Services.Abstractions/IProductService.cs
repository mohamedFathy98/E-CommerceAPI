global using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        // Get All Products 
        public Task<IEnumerable<ProductResultDTO>> GetAllProductsAsync();
        //Get all brands
        public Task<IEnumerable<BrandResultDTO>> GetAllbrandResultAsync();

        // Get all types
        public Task<IEnumerable<TypeResultDTO>> GetAllTypeResultAsync();
        //Get product By Id
        public Task<IEnumerable<ProductResultDTO>> GetAllProductByIdAsync(int id);

    }
}
