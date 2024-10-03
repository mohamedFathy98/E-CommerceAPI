using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecification : Specifications<Product>
    {
        //use to reteieve product by id
        public ProductWithBrandAndTypeSpecification(int id) : base(product => product.Id == id)
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);
        }

        // use all products
        public ProductWithBrandAndTypeSpecification(string? sort, int? barndId, int? TypeId) :
            base(product => (!barndId.HasValue || product.BrandId == barndId.Value) && (!TypeId.HasValue || product.TypeId == TypeId.Value))
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);


            if (string.IsNullOrWhiteSpace(sort))
            {
                switch (sort.ToLower().Trim())
                {
                    case "pricedesc":
                        SetOrderByDescending(p => p.Price); break;
                    case "priceasc":
                        SetOrderBy(p => p.Price); break;
                    case "namedesc":
                        SetOrderByDescending(p => p.Name); break;

                    default:
                        SetOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}
