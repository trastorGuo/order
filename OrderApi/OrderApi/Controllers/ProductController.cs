using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                             from dtl in db.FoodDetails.LeftJoin(pr=> pr.FoodId == food.ID) 
                             from shopImg in db.Images.LeftJoin(pr=>pr.ConnectId == p.ID)
                             from img in db.Images.LeftJoin(pr=>pr.ConnectId == dtl.ID)
                             from foodImg in db.Images.LeftJoin(pr=> pr.ConnectId == food.ID)
                             where p.ACCOUNT == account
                             select new
                             {
                                 SHOP_ID = p.ID,
                                 SHOP_NAME = p.NAME,
                                 SHOP_IMG_ID = shopImg.ID,
                                 SHOP_IMG_URL = shopImg.URL,
                                 TYPE_ID = type.ID,
                                 type.ICON,
                                 type.TypeName,
                                 FOOD_ID = food.ID,
                                 food.NAME,
                                 FOOD_IMG_URL = foodImg.URL,
                                 FOOD_IMG_URL_ID = foodImg.ID,
                                 food.TAG,
                                 DETAIL_ID = dtl.ID,
                                 DETAIL_NAME = dtl.NAME,
                                 dtl.PRICE,
                                 dtl.DetailDesc,
                                 img.URL,
                                 DETAIL_IMG_ID = img.ID
                             };
                var rs = result.ToList().GroupBy(x => new { x.SHOP_NAME, x.SHOP_ID}).Select(x => new ProductModel
                {
                    SHOP_NAME = x.Key.SHOP_NAME,
                    SHOP_ID = x.Key.SHOP_ID,
                    Urls = x.GroupBy(c=>new { c.SHOP_IMG_ID, c.SHOP_IMG_URL }).Select(c=> new ProductImage
                    {
                        URL = c.Key.SHOP_IMG_URL,
                        IMG_ID = c.Key.SHOP_IMG_ID
                    }).ToList(),
                    TYPES = x.ToList().GroupBy(c => new { c.ICON, c.TypeName, c.TYPE_ID })
                    .Select(c => new ProductType
                    {
                        TYPE_ID = c.Key.TYPE_ID,
                        ICON = c.Key.ICON,
                        TYPE_NAME = c.Key.TypeName,
                        FOODS = c.GroupBy(z => new { z.NAME, z.TAG, z.FOOD_ID })
                        .Select(z => new ProductFood
                        {
                            FOOD_ID = z.Key.FOOD_ID,
                            FOOD_TAG = z.Key.TAG,
                            FOOD_NAME = z.Key.NAME,
                            Urls = z.GroupBy(d => new { d.FOOD_IMG_URL, d.FOOD_IMG_URL_ID })
                            .Select(d=> new ProductImage
                            {
                                URL = d.Key.FOOD_IMG_URL,
                                IMG_ID = d.Key.FOOD_IMG_URL_ID
                            }).ToList(),
                            FOOD_DETAIL = z.GroupBy(d => new { d.DETAIL_NAME, d.PRICE, d.DetailDesc, d.DETAIL_ID }).OrderBy(d=>d.Key.PRICE)
                            .Select(d => new ProductDetail
                            {
                                DETAIL_ID = d.Key.DETAIL_ID,
                                DETAIL_NAME = d.Key.DETAIL_NAME,
                                DETAIL_DESC = d.Key.DetailDesc,
                                DETAIL_PRICE = d.Key.PRICE ?? 0,
                                Urls = d.GroupBy(y=> new { y.URL, y.DETAIL_IMG_ID })
                                .Select(y=> new ProductImage
                                {
                                    IMG_ID = y.Key.DETAIL_IMG_ID,
                                    URL = y.Key.URL
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).FirstOrDefault();

                return rs;
            }
        }


        [Auth]
        [HttpPost]
        public object EditProduct(JToken jt)
        {
            var model = JsonConvert.DeserializeObject<EditProduct>(jt.ToString());
            using(var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    
                    var food = (from p in db.Foods where model.Food.FOOD_ID == p.ID select p).FirstOrDefault();
                    food.TypeId = model.Type.TYPE_ID;
                    food.NAME = model.Food.FOOD_NAME;
                    food.TAG = model.Food.FOOD_TAG;
                    food.DatetimeModified = DateTime.Now;
                    food.UserModified = ACCOUNT;
                    db.Update(food);
                    //替换食物图片
                    foreach(var id in model.Food.Urls)
                    {
                        var img = (from p in db.Images where id.IMG_ID == p.ID select p).FirstOrDefault();
                        img.DatetimeModified = DateTime.Now;
                        img.UserModified = ACCOUNT = ACCOUNT;
                        img.URL = id.URL;
                        db.Update(img);
                    }

                    //更新明细
                    foreach(var detail in model.Food.FOOD_DETAIL)
                    {
                        var dtl = (from p in db.FoodDetails where detail.DETAIL_ID == p.ID select p).FirstOrDefault();
                        dtl.DatetimeModified = DateTime.Now;
                        dtl.UserModified = ACCOUNT;
                        dtl.NAME = detail.DETAIL_NAME;
                        dtl.PRICE = detail.DETAIL_PRICE;
                        db.Update(dtl);

                        //更新明细图片
                        foreach(var id in detail.Urls)
                        {
                            var img = (from p in db.Images where id.IMG_ID == p.ID select p).FirstOrDefault();
                            img.DatetimeModified = DateTime.Now;
                            img.UserModified = ACCOUNT = ACCOUNT;
                            img.URL = id.URL;
                            db.Update(img);
                        }
                    }

                    db.CommitTransaction();
                }
                catch(Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }

                return true;
            }
        }



        [Auth]
        [HttpPost]
        public object AddProduct(JToken jt)
        {
            var model = JsonConvert.DeserializeObject<EditProduct>(jt.ToString());
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var food = new FOOD();
                    food.ID = Guid.NewGuid().ToString("N").ToUpper();
                    //插入食物图片
                    foreach (var id in model.Food.Urls)
                    {
                        var img = new IMAGE();
                        img.ID = Guid.NewGuid().ToString("N").ToUpper();
                        img.DatetimeCreated = DateTime.Now;
                        img.STATE = 'A';
                        img.UserCreated = ACCOUNT = ACCOUNT;
                        img.URL = id.URL;
                        img.ConnectId = food.ID;
                        db.Insert(img);
                    }
                    
                    food.UserCreated = ACCOUNT;
                    food.DatetimeCreated = DateTime.Now;
                    food.STATE = 'A';
                    food.TypeId = model.Type.TYPE_ID;
                    food.NAME = model.Food.FOOD_NAME;
                    food.TAG = model.Food.FOOD_TAG;
                    db.Insert(food);
                    

                    //更新明细
                    foreach (var detail in model.Food.FOOD_DETAIL)
                    {
                        var dtl = new FoodDetail();
                        dtl.ID = Guid.NewGuid().ToString("N").ToUpper();
                        dtl.DatetimeCreated = DateTime.Now;
                        dtl.UserCreated = ACCOUNT;
                        dtl.FoodId = food.ID;
                        dtl.NAME = detail.DETAIL_NAME;
                        dtl.PRICE = detail.DETAIL_PRICE;
                        dtl.STATE = 'A';
                        db.Insert(dtl);

                        //更新明细图片
                        foreach (var id in detail.Urls)
                        {
                            var img = new IMAGE();
                            img.ID = Guid.NewGuid().ToString("N").ToUpper();
                            img.DatetimeCreated = DateTime.Now;
                            img.UserCreated = ACCOUNT = ACCOUNT;
                            img.ConnectId = dtl.ID;
                            img.URL = id.URL;
                            img.STATE = 'A';
                            db.Insert(img);
                        }
                    }

                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }

                return true;
            }
        }

        [HttpPost]
        public object AddType(JToken jt)
        {
            var model = JsonConvert.DeserializeObject<ProductType>(jt.ToString());
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var type = new FoodType();
                    type.ID = Guid.NewGuid().ToString("N").ToUpper();
                    type.DatetimeCreated = DateTime.Now;
                    type.UserCreated = ACCOUNT;
                    type.ICON = model.ICON;
                    type.ShopId = SHOP_ID;
                    type.TypeName = model.TYPE_NAME;
                    type.STATE = 'A';
                    db.Insert(type);

                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }

                return true;
            }
        }


        [HttpPost]
        public object PlaceAnOrder(JToken jt)
        {
            var model = JsonConvert.DeserializeObject<PlaceAnOrder>(jt.ToString());
            using(var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var shopId = (from p in db.Shops where p.ACCOUNT == model.Account select p).FirstOrDefault()?.ID;
                    if (string.IsNullOrEmpty(model.OrderId))
                    {
                        var head = new OrderHead()
                        {
                            ID = Guid.NewGuid().ToString("N").ToUpper(),
                            DatetimeCreated = DateTime.Now,
                            UserCreated = "custom",
                            ShopId = shopId,
                            STATE = 'A',
                            DescNum = model.DescNum,
                            IsClose = 'N'
                        };
                        db.Insert(head);
                        model.OrderId = head.ID;
                    }

                    var orderDetail = new OrderDetail()
                    {
                        ID = Guid.NewGuid().ToString("N").ToUpper(),
                        DatetimeCreated = DateTime.Now,
                        UserCreated = "custom",
                        STATE = 'A',
                        UserOrder = model.User,
                        PrrentOrderId = model.OrderId
                    };
                    db.Insert(orderDetail);

                    foreach (var item in model.Foods)
                    {
                        var foodDetail = new OrderDetailFood()
                        {
                            ID = Guid.NewGuid().ToString("N").ToUpper(),
                            DatetimeCreated = DateTime.Now,
                            UserCreated = "custom",
                            STATE = 'A',
                            UserOrder = model.User,
                            OrderDetailId = orderDetail.ID,
                            FoodDetailId = item.DETAIL_ID,
                            QTY = item.NUM
                        };
                        db.Insert(foodDetail);
                    }
                    db.CommitTransaction();
                    return model.OrderId;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                    db.RollbackTransaction();
                }
            }
        }


        /// <summary>
        /// 判断当前桌是否有人正在占用 true:被占用
        /// </summary>
        /// <param name="desckNum"></param>
        /// <param name="shopAcount"></param>
        /// <returns></returns>
        [HttpGet]
        public bool DeskIsOccupied(string desckNum, string shopAcount)
        {
            using(var db = new OrderDB())
            {
                var shopId = (from p in db.Shops where p.ACCOUNT == shopAcount select p).FirstOrDefault()?.ID;
                var order = (from p in db.OrderHeads
                             orderby p.DatetimeCreated descending
                             where p.DescNum == desckNum && p.ShopId == shopId
                             select p).FirstOrDefault();
                return order != null && order.IsClose == 'N';
            }
        }



        [HttpGet]
        public bool CloseOrder(string orderId)
        {
            using (var db = new OrderDB())
            {
                try
                {
                    var order = (from p in db.OrderHeads
                                 where p.ID == orderId
                                 select p).FirstOrDefault();
                    order.IsClose = 'Y';
                    order.DatetimeModified = DateTime.Now;
                    db.Update(order);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return true;
            }
        }


        [HttpGet]
        [Auth]
        public object GetDeskList()
        {
            using(var db = new OrderDB())
            {
                var result = from p in db.ShopDesks where p.ShopId == SHOP_ID select p;
                return result.ToList();
            }
        }


        [HttpGet]
        [Auth]
        public void AddOrEditDesk(string deskNum, string deskDesc)
        {
            using (var db = new OrderDB())
            {
                var result = from p in db.ShopDesks where p.DeskCount == deskNum select p;
                if(result is null || result.Count() == 0)
                {
                    var deck = new ShopDesk()
                    {
                        ID = Guid.NewGuid().ToString("N").ToUpper(),
                        DatetimeCreated = DateTime.Now,
                        STATE = 'A',
                        UserCreated = ACCOUNT,
                        DeskCount = deskNum,
                        DescDesc = deskDesc
                    };
                    db.Insert(deck);
                }
                else
                {
                    var desk = result.FirstOrDefault();
                    desk.DatetimeModified = DateTime.Now;
                    desk.UserModified = ACCOUNT;
                    desk.DeskCount = deskNum;
                    desk.DescDesc = deskDesc;
                    db.Update(desk);
                }
            }
        }
    }
}
