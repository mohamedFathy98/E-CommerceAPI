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
        public ProductWithBrandAndTypeSpecification(ProductSpecificationsParameters parameters) :
            base(product => (!parameters.BrandId.HasValue || product.BrandId == parameters.BrandId) && 
            (!parameters.TypeId.HasValue || product.TypeId == parameters.TypeId))
        {
            AddInclude(product => product.ProductBrand);
            AddInclude(product => product.ProductType);

            ApplyPagination(parameters.pageIndex, parameters.pageSize);

            if (parameters.Sort is not null)
            {
                switch (parameters.Sort)
                {
                    case ProductSortingOptions.NameDesc:
                        SetOrderByDescending(p => p.Name); break;
                    case ProductSortingOptions.NameAsc:
                        SetOrderBy(p => p.Name); break;
                    case ProductSortingOptions.PriceDesc:
                        SetOrderByDescending(p => p.Price); break;
                    case ProductSortingOptions.PriceAsc:
                        SetOrderBy(p => p.Price); break;


                    default:

                        break;
                }
            }
        }
    }
}
