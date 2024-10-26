using Domain.Entites.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderWithPaymentIntentIdSpecification : Specifications<Order>
    {
        public OrderWithPaymentIntentIdSpecification(string paymentIntentId)
            : base(order => order.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
