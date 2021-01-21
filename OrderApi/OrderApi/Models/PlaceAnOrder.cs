﻿using System;
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
        public int DescNum { get; set; }
    }

    public class OrderDetail
    {
        public string DETAIL_ID { get; set; }
        public int NUM { get; set; }
    }
}
