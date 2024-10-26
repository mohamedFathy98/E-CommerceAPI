global using Product = Domain.Entites.Product;
using Domain.Entites.OrderEntities;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class PaymentService(IBasketRepository basketRepository,
          IUnitOfWork unitOfWork, IMapper mapper
          , IConfiguration configuration) : IPaymentService
    {
        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = configuration.GetRequiredSection("StripeSettings")["SecretKey"];
            // Get Basket => SubTotal => Product
            var basket = await basketRepository.GetBasketAsync(basketId)
            ?? throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;
            }

            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No Delivery Method is Selected");

            var method = await unitOfWork.GetRepository<DeliveryMethod, int>()
                   .GetAsync(basket.DeliveryMethodId.Value)
                   ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = method.Price;

            var amount = (long)(basket.Items.Sum(item => item.Quantity * item.Price) + basket.ShippingPrice) * 100;
            var service = new PaymentIntentService();
            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId))
            {
                //Create
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }

                };
                var paymentIntent = await service.CreateAsync(createOptions);


                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                //Update
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                };
                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }
            await basketRepository.UpdaateBasketAsync(basket);
            return mapper.Map<BasketDTO>(basket);
        }
    }
}
