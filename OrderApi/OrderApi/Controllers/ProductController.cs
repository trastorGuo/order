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
                             join img in db.Images on dtl.ID equals img.ConnectId
                             where p.ACCOUNT == account
                             select new
                             {
                                 p.ACCOUNT,
                                 TYPE_ID = type.ID,
                                 type.ICON,
                                 type.TypeName,
                                 FOOD_ID=food.ID,
                                 food.NAME,
                                 food.ImgId,
                                 food.TAG,
                                 DETAIL_ID = dtl.ID,
                                 DETAIL_NAME = dtl.NAME,
                                 dtl.PRICE,
                                 dtl.DetailDesc,
                                 img.URL
                             };
                var rs = result.ToList().GroupBy(x => x.ACCOUNT).Select(x => new ProductModel
                {
                    SHOP_NAME = x.Key,
                    TYPES = x.ToList().GroupBy(c => new { c.ICON, c.TypeName, c.TYPE_ID })
                    .Select(c => new ProductType
                    {
                        TYPE_ID = c.Key.TYPE_ID,
                        ICON = c.Key.ICON,
                        TYPE_NAME = c.Key.TypeName,
                        FOODS = c.GroupBy(z => new { z.NAME, z.TAG, z.ImgId, z.FOOD_ID })
                        .Select(z => new ProductFood
                        {
                            FOOD_ID = z.Key.FOOD_ID,
                            FOOD_TAG = z.Key.TAG,
                            FOOD_IMG_ID = z.Key.ImgId,
                            FOOD_NAME = z.Key.NAME,
                            FOOD_DETAIL = z.GroupBy(d => new { d.DETAIL_NAME, d.PRICE, d.DetailDesc, d.DETAIL_ID })
                            .Select(d => new ProductDetail
                            {
                                DETAIL_ID = d.Key.DETAIL_ID,
                                DETAIL_NAME = d.Key.DETAIL_NAME,
                                DETAIL_DESC = d.Key.DetailDesc,
                                DETAIL_PRICE = d.Key.PRICE ?? 0,
                                Urls = d.GroupBy(y=>y.URL)
                                .Select(y=> new ProductImage
                                {
                                    URL = y.Key
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();

                return rs;
            }
        }
    }
}
