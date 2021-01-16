using Microsoft.AspNetCore.Mvc;
using OrderApi.Domains;
using OrderApi.Models;
using OrderApi.MsgCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [WebApi]
    public class ProductController : BaseController
    {
        [HttpGet]
        public object GetProductList(string account)
        {
            using(var db = new OrderDB())
            {
                var result = from p in db.Shops
                             join type in db.FoodTypes on p.ID equals type.ShopId
                             join food in db.Foods on type.ID equals food.TypeId
                             join dtl in db.FoodDetails on food.ID equals dtl.FoodId
                             where p.ACCOUNT == account
                             select new
                             {
                                 p.ACCOUNT,
                                 type.ICON,
                                 type.TypeName,
                                 food.NAME,
                                 food.ImgId,
                                 food.TAG,
                                 DETAIL_NAME = dtl.NAME,
                                 dtl.PRICE,
                                 dtl.DetailDesc
                             };
                var rs = result.ToList().GroupBy(x => x.ACCOUNT).Select(x => new ProductModel
                {
                    SHOP_NAME = x.Key,
                    TYPES = x.ToList().GroupBy(c => new { c.ICON, c.TypeName })
                    .Select(c => new ProductType
                    {
                        ICON = c.Key.ICON,
                        TYPE_NAME = c.Key.TypeName,
                        FOODS = c.GroupBy(z => new { z.NAME, z.TAG, z.ImgId })
                        .Select(z => new ProductFood
                        {
                            FOOD_TAG = z.Key.TAG,
                            FOOD_IMG_ID = z.Key.ImgId,
                            FOOD_NAME = z.Key.NAME,
                            FOOD_DETAIL = z.GroupBy(d => new { d.DETAIL_NAME, d.PRICE, d.DetailDesc })
                            .Select(d => new ProductDetail
                            {
                                DETAIL_NAME = d.Key.DETAIL_NAME,
                                DETAIL_DESC = d.Key.DetailDesc,
                                DETAIL_PRICE = d.Key.PRICE ?? 0
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

                return rs;
            }
        }
    }
}
