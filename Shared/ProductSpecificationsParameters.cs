using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationsParameters
    {
        private const int MAXPAGESIZE = 10;
        private const int DEFALTPAGESIZE = 5;
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }
        public ProductSortingOptions? Sort { get; set; }
        public int pageIndex { get; set; } = 1;

        private int _pageSize = DEFALTPAGESIZE;
        public int pageSize
        {
            get => _pageSize;
            set => _pageSize = value > MAXPAGESIZE ? MAXPAGESIZE : value;
        }
        public string? Search { get; set; }
    }
    public enum ProductSortingOptions
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc,
    }
}
