using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrderModels
{
    public record OrderResult
    {
        public Guid Id { get; set; }
        //User Email
        public string UserEamil { get; set; }

        //Address
        public AddressDTO ShippingAddress { get; set; }

        //OrdderItems
        public ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();

        //Payment Status

        public string PaymentStatus { get; set; }

        //Delivery Method

        public string DeliveryMethod { get; set; }


        // SubTotal => items.Q * Price

        public decimal SubTotal { get; set; }

        //Payment

        public string PaymentIntentId { get; set; } = string.Empty;

        //Order Date
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public decimal Total { get; set; }

    }
}
