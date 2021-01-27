using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class ProductModel
    {
        public string SHOP_NAME { get; set; }
        public string SHOP_ID { get; set; }
        public List<ProductType> TYPES { get; set; }
        public List<ProductImage> Urls { get; set; }
    }

    public class ProductType
    {
        public int SEQ { get; set; }
        public string TYPE_ID { get; set; }
        public string ICON { get; set; }
        public string TYPE_NAME { get; set; }
        public List<ProductFood> FOODS { get; set; }
    }

    public class ProductFood
    {
        public string FOOD_ID { get; set; }
        public string FOOD_TAG { get; set; }
        public string FOOD_NAME { get; set; }
        public List<ProductDetail> FOOD_DETAIL { get; set; }
        public List<ProductImage> Urls { get; set; }
    }

    public class ProductDetail
    {
        public string DETAIL_ID { get; set; }
        public string DETAIL_NAME { get; set; }
        public string DETAIL_DESC { get; set; }
        public decimal DETAIL_PRICE { get; set; }
        public List<ProductImage> Urls { get; set; }
    }

    public class ProductImage
    {
        public string IMG_ID { get; set; }
        public string URL { get; set; }
    }

    public class EditProduct
    {
        public ProductType Type { get; set; }
        public ProductFood Food { get; set; }
    }
}
