using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IServicesManger
    {
        public IProductService productService { get; }
        public IBasketService BasketService { get; }
        public IAuthenticationService AuthenticationService { get; }
    }

}
