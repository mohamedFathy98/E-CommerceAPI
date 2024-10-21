using Shared.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IOrderService
    { 
        //Get Order By Id => OrderResult(Guid id)
        public Task<OrderResult> GetOrderByIdAsync(Guid id);
        // Get orders for user by email
        public Task<IEnumerable<OrderResult>> GetOrderByEmailAsync(string email);
        // Create order
        public Task<OrderResult> CreateOrderAsync(OrderRequest orderRequest, string userEmail);
        //Get all Delivery methods
        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();


    }
}
