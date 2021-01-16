using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class ProductModel
    {
        public string SHOP_NAME { get; set; }
        public List<ProductType> TYPES { get; set; }
    }

    public class ProductType
    {
        public string ICON { get; set; }
        public string TYPE_NAME { get; set; }
        public List<ProductFood> FOODS { get; set; }
    }

    public class ProductFood
    {
        public string FOOD_TAG { get; set; }
        public string FOOD_NAME { get; set; }
        public string FOOD_IMG_ID { get; set; }
        public List<ProductDetail> FOOD_DETAIL { get; set; }
    }

    public class ProductDetail
    {
        public string DETAIL_NAME { get; set; }
        public string DETAIL_DESC { get; set; }
        public decimal DETAIL_PRICE { get; set; }
    }
}
