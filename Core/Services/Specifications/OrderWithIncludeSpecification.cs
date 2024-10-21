using Domain.Entites.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class OrderWithIncludeSpecification : Specifications<Order>
    {
        public OrderWithIncludeSpecification(Guid id)
            : base(order => order.Id == id)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.OrderItems);

        }
        public OrderWithIncludeSpecification(string email)
           : base(order => order.UserEamil == email)
        {
            AddInclude(order => order.DeliveryMethod);
            AddInclude(order => order.OrderItems);

            SetOrderBy(o => o.OrderDate);


        }
    }
    }
