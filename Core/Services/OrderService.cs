using Domain.Entites.OrderEntities;
using Domain.Entites;
using Domain.Exceptions;
using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Specifications;

namespace Services
{
    internal class OrderService  (IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository) : IOrderService
    {
        public async Task<OrderResult> CreateOrUpdateOrderAsync(OrderRequest orderRequest, string userEmail)
        {
            //1. Address
            var address = mapper.Map<Domain.Entites.OrderEntities.Address>(orderRequest.ShippingAddress);
            //2. order Items => Basket => Basket items => order Items
            var basket = await basketRepository.GetBasketAsync(orderRequest.BasketId)
            ?? throw new BasketNotFoundException(orderRequest.BasketId);
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
              .GetAsync(orderRequest.DeliveryMethodId) ?? throw new DeliveryMethodNotFoundException(orderRequest.DeliveryMethodId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                orderItems.Add(CreateOrderItem(item, product));
            }
     
          
            var orderRepo = unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await orderRepo.GetAsync(new OrderWithPaymentIntentIdSpecification(
           basket.PaymentIntentId!));
            if (existingOrder is not null)
            {
                orderRepo.Delete(existingOrder);
            }
            // 4.sub Total

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);


            // save to db
            var order = new Order(userEmail, address, orderItems, deliveryMethod, subtotal,basket.PaymentIntentId!);
            await orderRepo.AddAsync(order);
            await unitOfWork.SaveChangesAsync();


            // map & return
            return mapper.Map<OrderResult>(order);
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
         => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl),
            item.Quantity, product.Price);

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var methods = await unitOfWork.GetRepository<DeliveryMethod, int>()
               .GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods);
        }

        public async Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
                       .GetAllAsync(new OrderWithIncludeSpecification(email));

            return mapper.Map<IEnumerable<OrderResult>>(orders);

        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithIncludeSpecification(id))
                ?? throw new OrderNotFoundException(id);
            return mapper.Map<OrderResult>(order);
        }
    }
}
