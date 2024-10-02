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

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper Mapper) : IProductService
    {

        public async Task<IEnumerable<BrandResultDTO>> GetAllbrandResultAsync()
        {
            //1.Retreve all brands => unitOfWork
            var brands = await unitOfWork.GetRepository<ProductBrand ,int>().GetAllAsync();
            //2. Map to brand ResulteDTO => Imapper
            var brandResult = Mapper.Map<IEnumerable<BrandResultDTO>>(brands);
            //3. Ret
            return brandResult;
        }

        public async Task<IEnumerable<ProductResultDTO>> GetAllProductByIdAsync(int id)
        {
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();
            var productResult = Mapper.Map<IEnumerable<ProductResultDTO>>(products);
            return productResult;
        }
        

        public async Task<IEnumerable<ProductResultDTO>> GetAllProductsAsync()
        {
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync();
            var productResult = Mapper.Map<IEnumerable<ProductResultDTO>>(products);
            return productResult;
        }

        public async Task<IEnumerable<TypeResultDTO>> GetAllTypeResultAsync()
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            var typesResult = Mapper.Map<IEnumerable<TypeResultDTO>>(types);
            return typesResult;
        }
    }
}
