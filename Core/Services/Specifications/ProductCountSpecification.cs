using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductCountSpecification : Specifications<Product>
    {
        public ProductCountSpecification(ProductSpecificationsParameters parameters) :
            base(product => (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) &&
           (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId) 
           )
        {


        }
    
    
    }
}
