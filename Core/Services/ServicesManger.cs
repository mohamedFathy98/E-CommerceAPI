using Domain.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServicesManger : IServicesManger
    {
        private readonly Lazy<IProductService> _productService;

        private readonly Lazy<IBasketService> _lazyBasketService;
        private readonly Lazy<IAuthenticationService> _lazyAuthenticationService;
        private readonly Lazy<IOrderService> _lazyOrderService;
        public ServicesManger(IUnitOfWork unitOfWork, IMapper mapper, IBasketRepository basketRepository, UserManager<User> userManager, IOptions<JwtOptions> options)
        {
            _productService = new Lazy<IProductService>(() => new ProductService(unitOfWork, mapper));
            _lazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, mapper));
            _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager,options));
            _lazyOrderService = new Lazy<IOrderService>(() =>
               new OrderService(unitOfWork, mapper, basketRepository));
        }
        public IProductService productService => _productService.Value;
        public IBasketService BasketService => _lazyBasketService.Value;
        public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;

        public IOrderService OrderService => _lazyOrderService.Value;
    }
}
