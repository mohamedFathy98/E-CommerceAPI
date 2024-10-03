﻿global using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IProductService
    {
        // Get All Product
        public Task<IEnumerable<ProductResultDTO>> GetAllProductsAsync(string? sort, int? barndId, int? TypeId);
        // Get All Brands
        public Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync();
        // Get All Type
        public Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync();
        // Get product by id
        public Task<ProductResultDTO> GetProductByIdAsync(int id);

    }
}
