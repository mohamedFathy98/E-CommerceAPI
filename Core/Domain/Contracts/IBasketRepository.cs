﻿using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string id);
        public Task<CustomerBasket?> UpdaateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null);
        public Task<bool> DeleteBasketAsync(string id);
    }
}
