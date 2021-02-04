using LinqToDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Domains
{
    public class ProductDomain
    {
        private static ProductDomain _current;
        public static ProductDomain Current = _current ?? new ProductDomain();


        public object GetProductList(string account)
        {
            using (var db = new OrderDB())
            {
                var result = from p in db.Shops
                             from type in db.FoodTypes.LeftJoin(pr => pr.ShopId == p.ID && pr.STATE == 'A')
                             from food in db.Foods.LeftJoin(pr => pr.TypeId == type.ID && pr.STATE == 'A')
                             from dtl in db.FoodDetails.LeftJoin(pr => pr.FoodId == food.ID && pr.STATE == 'A')
                             from shopImg in db.Images.LeftJoin(pr => pr.ConnectId == p.ID && pr.STATE == 'A')
                             from img in db.Images.LeftJoin(pr => pr.ConnectId == dtl.ID && pr.STATE == 'A')
                             from foodImg in db.Images.LeftJoin(pr => pr.ConnectId == food.ID && pr.STATE == 'A')
                             where p.ACCOUNT == account && p.STATE == 'A'
                             select new
                             {
                                 SHOP_ID = p.ID,
                                 SHOP_NAME = p.NAME,
                                 SHOP_IMG_ID = shopImg.ID,
                                 SHOP_IMG_URL = shopImg.URL,
                                 TYPE_ID = type.ID,
                                 type.ICON,
                                 type.TypeName,
                                 SEQ = type == null ? (int?)(null) : type.SEQ,
                                 FOOD_ID = food.ID,
                                 food.NAME,
                                 FOOD_IMG_URL = foodImg.URL,
                                 FOOD_IMG_URL_ID = foodImg.ID,
                                 food.TAG,
                                 food.INVENTORY,
                                 food.VISIBLE,
                                 DETAIL_ID = dtl.ID,
                                 DETAIL_NAME = dtl.NAME,
                                 dtl.PRICE,
                                 dtl.DetailDesc,
                                 img.URL,
                                 DETAIL_IMG_ID = img.ID
                             };
                var rs = result.ToList().GroupBy(x => new { x.SHOP_NAME, x.SHOP_ID }).Select(x => new ProductModel
                {
                    SHOP_NAME = x.Key.SHOP_NAME,
                    SHOP_ID = x.Key.SHOP_ID,
                    Urls = result.All(c => c.SHOP_IMG_ID == null) ? new List<ProductImage>() : x.GroupBy(c => new { c.SHOP_IMG_ID, c.SHOP_IMG_URL }).Select(c => new ProductImage
                    {
                        URL = c.Key.SHOP_IMG_URL,
                        IMG_ID = c.Key.SHOP_IMG_ID
                    }).ToList(),
                    TYPES = x.All(c => c.TYPE_ID == null) ? new List<ProductType>() : x.ToList().GroupBy(c => new { c.ICON, c.TypeName, c.TYPE_ID, c.SEQ }).OrderBy(c => c.Key.SEQ)
                     .Select(c => new ProductType
                     {
                         SEQ = c.Key.SEQ,
                         TYPE_ID = c.Key.TYPE_ID,
                         ICON = c.Key.ICON,
                         TYPE_NAME = c.Key.TypeName,
                         FOODS = c.Count(x => x.FOOD_ID != null) == 0 ? new List<ProductFood>() : c.GroupBy(z => new { z.NAME, z.TAG, z.FOOD_ID, z.VISIBLE, z.INVENTORY})
                         .Select(z => new ProductFood
                         {
                             FOOD_ID = z.Key.FOOD_ID,
                             FOOD_TAG = z.Key.TAG,
                             FOOD_NAME = z.Key.NAME,
                             VISIBLE = z.Key.VISIBLE,
                             INVENTORY = z.Key.INVENTORY,
                             Urls = z.All(d => d.FOOD_IMG_URL_ID == null) ? new List<ProductImage>() : z.GroupBy(d => new { d.FOOD_IMG_URL, d.FOOD_IMG_URL_ID })
                             .Select(d => new ProductImage
                             {
                                 URL = d.Key.FOOD_IMG_URL,
                                 IMG_ID = d.Key.FOOD_IMG_URL_ID
                             }).ToList(),
                             FOOD_DETAIL = z.All(d => d.DETAIL_ID == null) ? new List<ProductDetail>() : z.GroupBy(d => new { d.DETAIL_NAME, d.PRICE, d.DetailDesc, d.DETAIL_ID }).OrderBy(d => d.Key.PRICE)
                             .Select(d => new ProductDetail
                             {
                                 DETAIL_ID = d.Key.DETAIL_ID,
                                 DETAIL_NAME = d.Key.DETAIL_NAME,
                                 DETAIL_DESC = d.Key.DetailDesc,
                                 DETAIL_PRICE = d.Key.PRICE ?? 0,
                                 Urls = d.All(y => y.DETAIL_IMG_ID == null) ? new List<ProductImage>() : d.GroupBy(y => new { y.URL, y.DETAIL_IMG_ID })
                                 .Select(y => new ProductImage
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


        public object EditProduct(JToken jt, string ACCOUNT)
        {
            var model = JsonConvert.DeserializeObject<EditProduct>(jt.ToString());
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {

                    var food = (from p in db.Foods where model.Food.FOOD_ID == p.ID && p.STATE == 'A' select p).FirstOrDefault();
                    if (food is null)
                    {
                        throw new Exception($"找不到商品ID:{model.Food.FOOD_ID}");
                    }
                    if (string.IsNullOrEmpty(model.Type.TYPE_ID))
                    {
                        throw new Exception("商品类别ID不能为空！");
                    }
                    food.TypeId = model.Type.TYPE_ID;
                    food.NAME = model.Food.FOOD_NAME;
                    food.TAG = model.Food.FOOD_TAG;
                    food.DatetimeModified = DateTime.Now;
                    food.UserModified = ACCOUNT;
                    food.INVENTORY = model.Food.INVENTORY;
                    food.VISIBLE = model.Food.VISIBLE;
                    db.Update(food);

                    //删除商品图片
                    var imgIds = model.Food.Urls.Select(x => x.IMG_ID);
                    db.Images.Where(x => imgIds.Contains(x.ID)).Delete();

                    //新增食物图片
                    foreach (var id in model.Food.Urls)
                    {
                        var img = new IMAGE();
                        img.ID = Guid.NewGuid().ToString("N").ToUpper();
                        img.UserCreated = ACCOUNT;
                        img.DatetimeCreated = DateTime.Now;
                        img.ConnectId = food.ID;
                        img.STATE = 'A';
                        img.URL = id.URL;
                        db.Insert(img);
                    }

                    //先删除明细，再新增
                    var ids = model.Food.FOOD_DETAIL.Select(x => x.DETAIL_ID);
                    db.FoodDetails.Where(x => x.FoodId == food.ID).Delete();
                    //删除明细图片
                    db.Images.Where(x => ids.Contains(x.ConnectId)).Delete();

                    //更新明细
                    foreach (var detail in model.Food.FOOD_DETAIL)
                    {
                        if (detail.DETAIL_NAME == null)
                        {
                            throw new Exception("明细名称不能为null，请尝试使用\"\"");
                        }
                        if (model.Food.FOOD_DETAIL.Count(x => x.DETAIL_NAME == detail.DETAIL_NAME) >= 2)
                        {
                            throw new Exception("商品明细不能重复！");
                        }
                        var dtl = new FoodDetail();
                        dtl.ID = Guid.NewGuid().ToString("N").ToUpper();
                        dtl.DatetimeCreated = DateTime.Now;
                        dtl.UserCreated = ACCOUNT;
                        dtl.STATE = 'A';
                        dtl.FoodId = food.ID;
                        dtl.NAME = detail.DETAIL_NAME;
                        dtl.PRICE = detail.DETAIL_PRICE;
                        dtl.DetailDesc = detail.DETAIL_DESC;
                        db.Insert(dtl);

                        //更新明细图片
                        foreach (var id in detail.Urls)
                        {
                            var img = new IMAGE();
                            img.ID = Guid.NewGuid().ToString("N").ToUpper();
                            img.STATE = 'A';
                            img.DatetimeCreated = DateTime.Now;
                            img.UserCreated = ACCOUNT;
                            img.ConnectId = dtl.ID;
                            img.URL = id.URL;
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


        public object DeleteProduct(string id)
        {
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    //先删除明细
                    var dtl = (from p in db.FoodDetails where p.ID == id && p.STATE == 'A' select p).FirstOrDefault();
                    var food = (from p in db.Foods where p.ID == id && p.STATE == 'A' select p).FirstOrDefault();
                    if (dtl != null)
                    {
                        dtl.STATE = 'D';
                        db.Update(dtl);
                    }
                    if (food != null)
                    {
                        var count = (from p in db.FoodDetails where p.ID == id && p.STATE == 'A' select p).Count();
                        if (count > 0) throw new Exception("请先删除明细");
                        else
                        {
                            food.STATE = 'D';
                            db.Update(food);
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


        public object AddProduct(JToken jt, string ACCOUNT)
        {
            var model = JsonConvert.DeserializeObject<EditProduct>(jt.ToString());
            if (string.IsNullOrEmpty(model.Type.TYPE_ID))
            {
                throw new Exception("类型ID为空！");
            }
            if (string.IsNullOrEmpty(model.Food.FOOD_NAME))
            {
                throw new Exception("商品名称不能为空！");
            }

            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var count = (from p in db.Foods where p.NAME == model.Food.FOOD_NAME && p.STATE == 'A' && p.TypeId == model.Type.TYPE_ID select p).Count();
                    if (count > 0)
                    {
                        throw new Exception("当前商品已存在");
                    }
                    var food = new FOOD();
                    food.ID = Guid.NewGuid().ToString("N").ToUpper();
                    //插入食物图片
                    if (model.Food.Urls != null && model.Food.Urls.Count > 0)
                    {
                        foreach (var id in model.Food.Urls)
                        {
                            var img = new IMAGE();
                            img.ID = Guid.NewGuid().ToString("N").ToUpper();
                            img.DatetimeCreated = DateTime.Now;
                            img.STATE = 'A';
                            img.UserCreated = ACCOUNT;
                            img.URL = id.URL;
                            img.ConnectId = food.ID;
                            db.Insert(img);
                        }
                    }

                    food.UserCreated = ACCOUNT;
                    food.DatetimeCreated = DateTime.Now;
                    food.STATE = 'A';
                    food.TypeId = model.Type.TYPE_ID;
                    food.NAME = model.Food.FOOD_NAME;
                    food.TAG = model.Food.FOOD_TAG;

                    food.VISIBLE = model.Food.VISIBLE;
                    food.INVENTORY = model.Food.INVENTORY;
                    db.Insert(food);

                    if (model.Food.FOOD_DETAIL != null && model.Food.FOOD_DETAIL.Count > 0)
                    {
                        //新增明细
                        foreach (var detail in model.Food.FOOD_DETAIL)
                        {
                            if (model.Food.FOOD_DETAIL.Count(x => x.DETAIL_NAME == detail.DETAIL_NAME) >= 2)
                            {
                                throw new Exception("商品明细不能重复!");
                            }
                            var dtl = new FoodDetail();
                            dtl.ID = Guid.NewGuid().ToString("N").ToUpper();
                            dtl.DatetimeCreated = DateTime.Now;
                            dtl.UserCreated = ACCOUNT;
                            dtl.FoodId = food.ID;
                            dtl.NAME = detail.DETAIL_NAME;
                            dtl.PRICE = detail.DETAIL_PRICE;
                            dtl.STATE = 'A';
                            db.Insert(dtl);

                            //新增明细图片
                            foreach (var id in detail.Urls)
                            {
                                var img = new IMAGE();
                                img.ID = Guid.NewGuid().ToString("N").ToUpper();
                                img.DatetimeCreated = DateTime.Now;
                                img.UserCreated = ACCOUNT;
                                img.ConnectId = dtl.ID;
                                img.URL = id.URL;
                                img.STATE = 'A';
                                db.Insert(img);
                            }
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


        public object AddType(JToken jt, string ACCOUNT, string SHOP_ID)
        {
            var model = JsonConvert.DeserializeObject<ProductType>(jt.ToString());
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    if (string.IsNullOrEmpty(model.TYPE_NAME))
                    {
                        throw new Exception("类别名称不能为空！");
                    }
                    //更新
                    db.FoodTypes
                        .Where(c => c.SEQ >= model.SEQ && c.ShopId == SHOP_ID && c.STATE == 'A')
                        .Set(c => c.SEQ, c => c.SEQ + 1)
                        .Update();

                    var repeat = (from p in db.FoodTypes where p.ShopId == SHOP_ID && p.TypeName == model.TYPE_NAME && p.STATE == 'A' select p).FirstOrDefault();
                    if (repeat != null)
                    {
                        throw new Exception($"当前分类{model.TYPE_NAME}已存在！");
                    }
                    var type = new FoodType();
                    type.ID = Guid.NewGuid().ToString("N").ToUpper();
                    type.DatetimeCreated = DateTime.Now;
                    type.UserCreated = ACCOUNT;
                    type.ICON = model.ICON;
                    type.ShopId = SHOP_ID;
                    type.TypeName = model.TYPE_NAME;
                    type.STATE = 'A';
                    type.SEQ = model.SEQ ?? 0;
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


        public object EditType(JToken jt, string ACCOUNT, string SHOP_ID)
        {
            var model = JsonConvert.DeserializeObject<ProductType>(jt.ToString());
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var curType = (from p in db.FoodTypes where p.ShopId == SHOP_ID && p.ID == model.TYPE_ID && p.STATE == 'A' select p).FirstOrDefault();
                    var changeType = (from p in db.FoodTypes where p.ShopId == SHOP_ID && p.SEQ == model.SEQ && p.STATE == 'A' select p).FirstOrDefault();
                    if (curType is null || changeType is null)
                    {
                        throw new Exception("交换失败!");
                    }
                    changeType.SEQ = curType.SEQ;
                    curType.SEQ = model.SEQ ?? 0;

                    curType.TypeName = model.TYPE_NAME;
                    curType.ICON = model.ICON;
                    curType.DatetimeModified = DateTime.Now;
                    curType.UserModified = ACCOUNT;

                    db.Update(curType);
                    db.Update(changeType);

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


        public object DeleteType(string id, string SHOP_ID)
        {
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var curType = (from p in db.FoodTypes where p.ID == id select p).FirstOrDefault();
                    if (curType is null)
                    {
                        throw new Exception("当前类别不存在！");
                    }
                    curType.STATE = 'D';

                    db.FoodTypes
                        .Where(c => c.SEQ > curType.SEQ && c.ShopId == SHOP_ID && c.STATE == 'A')
                        .Set(c => c.SEQ, c => c.SEQ - 1)
                        .Update();

                    db.Update(curType);
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


        public object PlaceAnOrder(JToken jt)
        {
            var model = JsonConvert.DeserializeObject<PlaceAnOrder>(jt.ToString());
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var shopId = (from p in db.Shops where p.ACCOUNT == model.Account select p).FirstOrDefault();
                    if (string.IsNullOrEmpty(model.OrderId))
                    {
                        var head = new OrderHead()
                        {
                            ID = Guid.NewGuid().ToString("N").ToUpper(),
                            DatetimeCreated = DateTime.Now,
                            UserCreated = "custom",
                            ShopId = shopId?.ID,
                            PersonNum = model.PersonNum,
                            STATE = 'A',
                            DescNum = model.DescNum,
                            IsClose = 'N',
                            IsPrint = Convert.ToChar(model.IsPrint)
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
                        var detailName = (from p in db.FoodDetails where p.ID == item.DETAIL_ID select p).FirstOrDefault();
                        
                        if (string.IsNullOrEmpty(item.DETAIL_ID))
                        {
                            throw new Exception("存在商品明细无ID");
                        }
                        var food = (from p in db.Foods where p.ID == detailName.FoodId && p.STATE == 'A' select p).FirstOrDefault();
                        if(food is null)
                        {
                            throw new Exception("当前商品不存在");
                        }
                        if (food.INVENTORY == 0)
                        {
                            throw new Exception($"商品{food.NAME}库存不足！");
                        }
                        var foodDetail = new OrderDetailFood()
                        {
                            ID = Guid.NewGuid().ToString("N").ToUpper(),
                            DatetimeCreated = DateTime.Now,
                            UserCreated = "custom",
                            STATE = 'A',
                            UserOrder = model.User,
                            OrderDetailId = orderDetail.ID,
                            FoodDetailName = string.IsNullOrEmpty(detailName.NAME) ? food.NAME : food.NAME + "(" + detailName.NAME + ")",
                            QTY = item.NUM,
                            Price = detailName.PRICE ?? 0
                        };
                        db.Insert(foodDetail);

                        food.INVENTORY -= 1;
                        db.Update(food);
                    }
                    db.CommitTransaction();
                    var msg = "";
                    if (!string.IsNullOrEmpty(shopId.PrinterCode) && model.IsPrint == "Y")
                    {
                        msg = PrinterDomain.Current.print(model);
                    }
                    return new
                    {
                        OrderId = model.OrderId,
                        PrintMsg = msg
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                    db.RollbackTransaction();
                }
            }
        }

        public object GetOrders(string id, string datetime, string to, string userOrdered, string SHOP_ID)
        {
            using (var db = new OrderDB())
            {
                var iquery = from order in db.OrderHeads
                             from dtl in db.OrderDetails.LeftJoin(pr => pr.PrrentOrderId == order.ID && pr.STATE == order.STATE)
                             from food in db.OrderDetailFoods.LeftJoin(pr => pr.OrderDetailId == dtl.ID && pr.STATE == order.STATE)
                             where (string.IsNullOrEmpty(id) ? order.STATE == 'A' : order.ID == id)
                             && (string.IsNullOrEmpty(datetime) ? order.STATE == 'A' : order.DatetimeCreated >= Convert.ToDateTime(datetime))
                             && (string.IsNullOrEmpty(to) ? order.STATE == 'A' : order.DatetimeCreated <= Convert.ToDateTime(to))
                             && (string.IsNullOrEmpty(userOrdered) ? order.STATE == 'A' : food.UserOrder == userOrdered)
                             && order.STATE == 'A'
                             && order.ShopId == SHOP_ID
                             select new
                             {
                                 DATETIME_CREATED = order.DatetimeCreated,
                                 ORDER_DATE = order.DatetimeCreated,
                                 ORDER_ID = order.ID,
                                 IS_CLOSE = order.IsClose,
                                 IS_PRINT = order.IsPrint,
                                 PERSON_NUM = order.PersonNum,
                                 DESC_NUM = order.DescNum,
                                 ORDER_DETAIL_ID = food.OrderDetailId,
                                 food.QTY,
                                 USER_ORDER = food.UserOrder,
                                 FOOD_DETAIL_NAME = food.FoodDetailName,
                                 PRICE = food.Price
                             };

                var result = iquery.AsEnumerable().GroupBy(x => x.DATETIME_CREATED.Date)
                    .Select(x => new
                    {
                        DATE = x.Key.Date,
                        ORDER = x.ToList().GroupBy(v => new { v.USER_ORDER, v.ORDER_ID, v.IS_CLOSE, v.IS_PRINT, v.PERSON_NUM, v.DESC_NUM, v.ORDER_DATE })
                        .Select(v => new
                        {
                            v.Key.ORDER_DATE,
                            v.Key.USER_ORDER,
                            v.Key.ORDER_ID,
                            v.Key.IS_PRINT,
                            v.Key.IS_CLOSE,
                            v.Key.PERSON_NUM,
                            v.Key.DESC_NUM,
                            FOODS = v.ToList().GroupBy(c => new { c.ORDER_DETAIL_ID, c.QTY, c.USER_ORDER, c.FOOD_DETAIL_NAME, c.PRICE })
                            .Select(c => new
                            {
                                c.Key.ORDER_DETAIL_ID,
                                c.Key.QTY,
                                c.Key.USER_ORDER,
                                c.Key.FOOD_DETAIL_NAME,
                                c.Key.PRICE
                            }).OrderByDescending(c=>c.PRICE).ToList()
                        })
                    }).ToList();

                return result.OrderByDescending(x=>x.DATE).ToList();
            }
        }


        public object DeleteOrder(string id)
        {
            using(var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var detail = from p in db.OrderDetails
                                 where p.PrrentOrderId == id && p.STATE == 'A'
                                 select p;
                    if(detail == null || !detail.Any())
                    {
                        throw new Exception("当前订单不存在");
                    }

                    var ids = detail.Select(x => x.ID);
                    var foods = from p in db.OrderDetailFoods where ids.Contains(p.OrderDetailId) && p.STATE == 'A' select p;

                    db.OrderHeads.Where(x => x.ID == id && x.STATE == 'A')
                        .Set(x => x.STATE, 'D').Update();

                    detail.ToList().ForEach(x =>
                    {
                        x.STATE = 'D';
                        x.DatetimeModified = DateTime.Now;
                    });

                    foods.ToList().ForEach(x =>
                    {
                        x.STATE = 'D';
                        x.DatetimeModified = DateTime.Now;
                    });

                    db.CommitTransaction();
                }
                catch(Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
                return "删除成功！";
            }
        }


        public string DeskIsOccupied(string desckNum, string shopAcount)
        {
            using (var db = new OrderDB())
            {
                var shopId = (from p in db.Shops where p.ACCOUNT == shopAcount select p).FirstOrDefault()?.ID;
                var order = (from p in db.OrderHeads
                             orderby p.DatetimeCreated descending
                             where p.DescNum == desckNum && p.ShopId == shopId
                             select p).FirstOrDefault();
                return (order != null && order.IsClose == 'N') ? order.ID : "";
            }
        }


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


        public string AddOrEditDesk(string deskNum, string deskDesc, string ACCOUNT, string SHOP_ID)
        {
            using (var db = new OrderDB())
            {
                var result = from p in db.ShopDesks where p.DeskCount == deskNum && p.ShopId == SHOP_ID && p.STATE == 'A' select p;
                if (result is null || result.Count() == 0)
                {
                    var deck = new ShopDesk()
                    {
                        ID = Guid.NewGuid().ToString("N").ToUpper(),
                        DatetimeCreated = DateTime.Now,
                        STATE = 'A',
                        ShopId = SHOP_ID,
                        UserCreated = ACCOUNT,
                        DeskCount = deskNum,
                        DescDesc = deskDesc
                    };
                    db.Insert(deck);
                    return "新增成功";
                }
                else
                {
                    var desk = result.FirstOrDefault();
                    desk.DatetimeModified = DateTime.Now;
                    desk.UserModified = ACCOUNT;
                    desk.DeskCount = deskNum;
                    desk.DescDesc = deskDesc;
                    db.Update(desk);
                    return "修改成功";
                }
            }
        }


        public string DeleteDesk(string descNum, string ACCOUNT, string SHOP_ID)
        {
            using (var db = new OrderDB())
            {
                var desck = from p in db.ShopDesks where p.ShopId == SHOP_ID && p.STATE == 'A' && p.DeskCount == descNum select p;
                desck.ToList().ForEach(x =>
                {
                    x.STATE = 'D';
                    x.DatetimeModified = DateTime.Now;
                    x.UserModified = ACCOUNT;
                    db.Update(x);
                });
                return "删除成功";
            }
        }

        public object Reprint(JToken jt)
        {
            var model = JsonConvert.DeserializeObject<PlaceAnOrder>(jt.ToString());
            var msg = PrinterDomain.Current.print(model);
            return new
            {
                PrintMsg = msg
            };
        }

    }
}
