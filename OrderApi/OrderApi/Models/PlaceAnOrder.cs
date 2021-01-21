using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Models
{
    public class PlaceAnOrder
    {
        public string OrderId { get; set; }
        public List<OrderDetail> foods { get; set; }
        public string account { get; set; }
        public string user { get; set; }
    }

    public class OrderDetail
    {
        public string DETAIL_ID { get; set; }
        public int NUM { get; set; }
    }
}
