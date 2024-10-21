﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.OrderEntities
{
 
    
        public class Order : BaseEntity<Guid>
        {
         

            //User Email
            public string UserEamil { get; set; }

            //Address
            public Address ShippingAddress { get; set; }

            //OrdderItems
            public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

            //Payment Status

            public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;

            //Delivery Method

            public DeliveryMethod DeliveryMethod { get; set; }
            public int? DeliveryMethodId { get; set; }

            // SubTotal => items.Q * Price

            public decimal SubTotal { get; set; }

            //Payment

            public string PaymentIntentId { get; set; } = string.Empty;

            //Order Date
            public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        }
        }
    
