using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class PlaceAnOrder
    {
        public string OrderId { get; set; }
        public List<OrderDetail> Foods { get; set; }
        public string Account { get; set; }
        public string User { get; set; }
        public string DescNum { get; set; }
        public string IsPrint { get; set; }
        public int PersonNum { get; set; }
        public string TEL { get; set; }
        public string CUSTOM_NAME { get; set; }
        public string MEMO { get; set; }
    }

    public class OrderDetail
    {
        public string DETAIL_ID { get; set; }
        public string DETAIL_NAME { get; set; }
        public int NUM { get; set; }
        public decimal? PRICE { get; set; }
    }
}
