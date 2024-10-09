global using Services.Abstractions;
global using AutoMapper;
global using Domain.Contracts;
global using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;
using Services.Specifications;

namespace Services
{
    internal class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDTO>> GetAllBrandsAsync()
        {
            //1-retrive all brands => unitofwork
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            //2- maap to brand resultDto=> IMapaer
            var barndResult = mapper.Map<IEnumerable<BrandResultDTO>>(brands);
            // return

            return barndResult;
        }

        public async Task<PaginatedResult<ProductResultDTO>> GetAllProductsAsync(ProductSpecificationsParameters parameters)
        {
            var products = await unitOfWork.GetRepository<Product, int>()
                  .GetAllAsync(new ProductWithBrandAndTypeSpecification(parameters));
            var productsResult = mapper.Map<IEnumerable<ProductResultDTO>>(products);
            var count = productsResult.Count();
            var TotalCount = await unitOfWork.GetRepository<Product, int>()
                .CountAsync(new ProductCountSpecification(parameters));
            var result = new PaginatedResult<ProductResultDTO>(parameters.pageIndex, count,
              TotalCount,
             productsResult);
            return result;
        }

        public async Task<IEnumerable<TypeResultDTO>> GetAllTypesAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typesResult = mapper.Map<IEnumerable<TypeResultDTO>>(types);
            return typesResult;
        }

        public async Task<ProductResultDTO> GetProductByIdAsync(int id)
        {
            var product = await unitOfWork.GetRepository<Product, int>()
                .GetAsync(new ProductWithBrandAndTypeSpecification(id));
            var productResult = mapper.Map<ProductResultDTO>(product);
            return productResult;
        }
    }
}
