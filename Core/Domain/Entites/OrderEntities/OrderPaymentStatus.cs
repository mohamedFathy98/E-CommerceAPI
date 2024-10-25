using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entites.OrderEntities
{
    public enum OrderPaymentStatus
    {
        Pending = 0,
        PaymentReceiveed = 1,
        PaymentFailed = 2
    }
}
