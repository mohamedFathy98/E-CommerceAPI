﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites
{
    public class CustomerBasket
    {
        public string Id { get; set; } //=>pk
        public IEnumerable<BasketItem> Items { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public int? DeliveryMethodId { get; set; }

        public decimal? ShippingPrice { get; set; }
    }
}
