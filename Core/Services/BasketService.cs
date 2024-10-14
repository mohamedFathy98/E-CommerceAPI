using Domain.Entites;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<bool> DeleteBasketAsync(string id)
     => await basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDTO> GetBasketAsync(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            return basket is null ? throw new BasketNotFoundException(id) :
                mapper.Map<BasketDTO>(basket);
        }

        public async Task<BasketDTO> UpdateBasketAsync(BasketDTO basket)
        {
            var coustmerBasket = mapper.Map<CustomerBasket>(basket);
            var updateBasket = await basketRepository.UpdaateBasketAsync(coustmerBasket);
            return updateBasket is null ?
                throw new Exception("Can not Update Basket Now !!") :
                mapper.Map<BasketDTO>(updateBasket);
        }
    }
}
