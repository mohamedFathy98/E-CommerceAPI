using Domain.Entites.OrderEntities;
using Domain.Entites;
using Domain.Exceptions;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class OrderService  (IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            //1. Address
            var address = mapper.Map<Domain.Entites.OrderEntities.Address>(orderRequest.ShippingAddress);
            //2. order Items => Basket => Basket items => order Items
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId)
            ?? throw new BasketNotFoundException(orderRequest.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, product));
            }
            // 3.Delivery 
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetAsync(orderRequest.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);
            // 4.sub Total
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);


            // save to db
            var order = new Order(userEmail, address, orderItems, deliveryMethod, subtotal);

            await unitOfWork.GetRepository<Order, Guid>()
                .AddAsync(order);
            await unitOfWork.SaveChangesAsync();


            // map & return
            return mapper.Map<OrderResult>(order);
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
         => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl),
            item.Quantity, product.Price);

        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
