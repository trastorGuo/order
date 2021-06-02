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

    [WebApi("api/[controller]/[action]")]
    public class UserController : BaseController
    {
        
        /// <summary>
        /// 登录校验成功返回token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        public string Login(string name, string pwd)
        {
            //校验用户
            if (!LoginDomain.Current.CheckPassword(name, pwd))
            {
                throw new Exception("当前用户名或密码不正确！");
            }
            var shopInfo = LoginDomain.Current.ShopInfo(name) as SHOP;
            //返回Token
            return AuthDomain.Current.BuildToken(name, shopInfo.IsAdmin, shopInfo.ID);
        }


        [HttpPost]
        public bool AddShop(JToken jt)
        {
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    if (!IS_ADMIN)
                    {
                        throw new Exception("当前用户无管理员权限!");
                    }
                    var name = jt["NAME"]?.ToString();
                    var address = jt["ADDRESS"]?.ToString();
                    var account = jt["ACCOUNT"]?.ToString();
                    var password = jt["PASSWORD"]?.ToString();
                    var tel = jt["TEL"]?.ToString();
                    var printer = jt["PRINTER"]?.ToString();
                    var urls = new List<IMAGE>();
                    var CAPITATION = jt["CAPITATION"]?.ToString();
                    var COST = Convert.ToInt32(string.IsNullOrEmpty(jt["COST"]?.ToString()) ? 0 : jt["COST"].ToString());
                    if (jt["URLS"]?.ToString() != null) urls = JsonConvert.DeserializeObject<List<IMAGE>>(jt["URLS"].ToString());
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("商店名称不能为空！");
                    }
                    if(string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                    {
                        throw new Exception("账号、密码不能为空");
                    }
                    if (LoginDomain.Current.UserIsExsist(name))
                    {
                        throw new Exception("名称已存在！");
                    }
                    var m = new SHOP();
                    m.ID = Guid.NewGuid().ToString("N").ToUpper();
                    m.UserCreated = ACCOUNT;
                    m.DatetimeCreated = DateTime.Now;
                    m.STATE = 'A';
                    m.NAME = name;
                    m.ACCOUNT = account;
                    m.ADDRESS = address;
                    m.PASSWORD = password;
                    m.TEL = tel;
                    m.PrinterCode = printer;
                    m.IsAdmin = "N";
                    m.CAPITATION = CAPITATION;
                    m.COST = COST;
                    foreach (var url in urls)
                    {
                        var u = new IMAGE
                        {
                            ID = Guid.NewGuid().ToString("N").ToUpper(),
                            UserCreated = ACCOUNT,
                            DatetimeCreated = DateTime.Now,
                            STATE = 'A',
                            URL = url.URL,
                            ConnectId = m.ID
                        };
                        db.Insert(u);
                    }
                    db.Insert(m);
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
        [Auth]
        public bool EditShop(JToken jt)
        {
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var name = jt["NAME"]?.ToString();
                    var address = jt["ADDRESS"]?.ToString();
                    var account = jt["ACCOUNT"]?.ToString();
                    var password = jt["PASSWORD"]?.ToString();
                    var tel = jt["TEL"]?.ToString();
                    var printer = jt["PRINTER"]?.ToString();
                    var CAPITATION = jt["CAPITATION"]?.ToString();
                    var COST = Convert.ToInt32(string.IsNullOrEmpty(jt["COST"]?.ToString()) ? 0 : jt["COST"].ToString());
                    var urls = JsonConvert.DeserializeObject<List<IMAGE>>(jt["URLS"].ToString());
                    var PARAMS = JsonConvert.DeserializeObject<List<ParamsValue>>(jt["PARAMS"].ToString());
                    var m = (from p in db.Shops where p.ACCOUNT == account select p).FirstOrDefault();
                    m.UserModified = ACCOUNT;
                    m.DatetimeModified = DateTime.Now;
                    m.STATE = 'A';
                    m.NAME = name;
                    m.ACCOUNT = account;
                    m.ADDRESS = address;
                    m.PASSWORD = password;
                    m.PrinterCode = printer;
                    m.TEL = tel;
                    m.CAPITATION = CAPITATION;
                    m.COST = COST;
                    foreach (var url in urls)
                    {
                        if (string.IsNullOrEmpty(url.ID))
                        {
                            var u = (from p in db.Images where p.ID == url.ID select p).FirstOrDefault();
                            u.UserModified = ACCOUNT;
                            u.DatetimeModified = DateTime.Now;
                            u.URL = url.URL;
                            db.Update(u);
                        }
                        else
                        {
                            var u = new IMAGE
                            {
                                ID = Guid.NewGuid().ToString("N").ToUpper(),
                                UserCreated = ACCOUNT,
                                DatetimeCreated = DateTime.Now,
                                STATE = 'A',
                                ConnectId = m.ID,
                                URL = url.URL
                            };
                            db.Insert(u);
                        }
                    }
                    foreach (var _sp in PARAMS)
                    {
                        var sysParam = (from p in db.SysParams where p.ShopId == m.ID && p.ParamName == _sp.PARAM_NAME select p).FirstOrDefault();
                        if (sysParam == null)
                        {
                            var sp = new SysParam
                            {
                                ID = Guid.NewGuid().ToString("N").ToUpper(),
                                UserCreated = ACCOUNT,
                                DatetimeCreated = DateTime.Now,
                                STATE = 'A',
                                ParamName = _sp.PARAM_NAME,
                                ShopId = m.ID
                            };
                            db.Insert(sp);
                            var val = new SysParamValue
                            {
                                ID = Guid.NewGuid().ToString("N").ToUpper(),
                                UserCreated = ACCOUNT,
                                DatetimeCreated = DateTime.Now,
                                STATE = 'A',
                                ParamValue = _sp.PARAM_VALUE,
                                ParamNameId = _sp.PARAM_NAME_ID
                            };
                            db.Insert(val);
                        }
                        else
                        {
                            sysParam.DatetimeModified = DateTime.Now;
                            sysParam.ParamName = _sp.PARAM_NAME;
                            db.Update(sysParam);

                            var val = (from p in db.SysParamValues where p.ParamNameId == sysParam.ID select p).FirstOrDefault();
                            if (val == null)
                            {
                                val = new SysParamValue
                                {
                                    ID = Guid.NewGuid().ToString("N").ToUpper(),
                                    UserCreated = ACCOUNT,
                                    DatetimeCreated = DateTime.Now,
                                    STATE = 'A',
                                    ParamValue = _sp.PARAM_VALUE,
                                    ParamNameId = _sp.PARAM_NAME_ID
                                };
                                db.Insert(val);
                            }
                            else
                            {
                                val.ParamValue = _sp.PARAM_VALUE;
                                val.DatetimeModified = DateTime.Now;
                                db.Update(val);
                            }
                        }
                    }

                    db.Update(m);
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

        [HttpGet]
        [Auth]
        public object ShopInfo(string account)
        {
            using (var db = new OrderDB()) {
                var shopInfo = LoginDomain.Current.ShopInfo(account);
                var urls = (from p in db.Images where p.ConnectId == shopInfo.ID && p.STATE == 'A' select p).Select(x => new
                {
                    ID = x.ID,
                    URL = x.URL
                }).ToList();
                var DeskList = (from p in db.ShopDesks where p.ShopId == SHOP_ID && p.STATE == 'A' select p).Select(x=>new
                {
                    x.DescDesc,
                    x.DeskCount
                }).ToList();
                var ps = from p in db.SysParams
                         from pv in db.SysParamValues.LeftJoin(c => c.ParamNameId == p.ID)
                         select new
                         {
                             PARAM_NAME_ID = p.ID,
                             PARAM_NAME = p.ParamName,
                             PARAM_VALUE = pv.ParamValue
                         };
                return new
                {
                    PRINTER = shopInfo.PrinterCode,
                    shopInfo.NAME,
                    shopInfo.ADDRESS,
                    shopInfo.ACCOUNT,
                    shopInfo.PASSWORD,
                    shopInfo.TEL,
                    URLS = urls,
                    shopInfo.CAPITATION,
                    shopInfo.COST,
                    PARAMS = ps.ToList(),
                    DeskList,
                    IS_ADMIN = shopInfo.IsAdmin == "Y",
                    IS_SHOW_MEMO = shopInfo.IsShowMemo == 'Y',
                    IS_SHOW_USER_COUNT = shopInfo.IsShowUserCount == null || shopInfo.IsShowUserCount == 'N'
                };
            }
        }



        [HttpPost]
        [Auth]
        public object ModifiefPassword(JToken jt)
        {
            string username = jt["username"]?.ToString();
            string passwordbefore = jt["passwordbefore"]?.ToString();
            string passwordafter = jt["passwordafter"]?.ToString();
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    if (IS_ADMIN)
                    {
                        db.Shops.Where(x => x.ACCOUNT == username && x.STATE == 'A')
                            .Set(x => x.PASSWORD, passwordafter)
                            .Update();
                    }
                    else
                    {
                        if (!LoginDomain.Current.CheckPassword(username, passwordbefore))
                        {
                            throw new Exception("当前用户名或密码不正确！");
                        }
                        else
                        {
                            db.Shops.Where(x => x.ACCOUNT == username && x.STATE == 'A')
                               .Set(x => x.PASSWORD, passwordafter)
                               .Update();
                        }
                    }
                    db.CommitTransaction();
                }
                catch(Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
                return "密码修改成功！";
            }
            
        }



        [HttpGet]
        [Auth]
        public object BusinessStatus()
        {
            using(var db = new OrderDB())
            {
                var bills = from order in db.OrderHeads
                            join dtl in db.OrderDetails on order.ID equals dtl.PrrentOrderId
                            join food in db.OrderDetailFoods on dtl.ID equals food.OrderDetailId
                            where order.ShopId == SHOP_ID && order.DatetimeCreated >= DateTime.Today.AddMonths(-6) && order.STATE == 'A'
                            select new
                            {
                                DATE = order.DatetimeCreated,
                                COST = order.COST,
                                ORDER_ID = order.ID,
                                FOOD_NAME = food.FoodDetailName,
                                PRICE = food.Price,
                                food.QTY,
                                PERSON_NUM = order.PersonNum
                            };

                var billsToday = bills.Where(x => x.DATE >= DateTime.Today).ToList();
                var salesToday = 0M;
                billsToday.GroupBy(x => new { x.ORDER_ID, x.COST, x.PERSON_NUM }).ToList().ForEach(x =>
                {
                    salesToday += x.Key.COST * (x.Key.PERSON_NUM ?? 0);
                    salesToday += x.Sum(c => c.PRICE * (c.QTY ?? 0));
                });

                var billsMonth = bills.Where(x => x.DATE >= DateTime.Today.AddMonths(-1)).ToList();
                var salesMonth = 0M;
                billsMonth.GroupBy(x => new { x.ORDER_ID, x.COST, x.PERSON_NUM }).ToList().ForEach(x =>
                {
                    salesMonth += x.Key.COST * (x.Key.PERSON_NUM ?? 0);
                    salesMonth += x.Sum(c => c.PRICE * (c.QTY ?? 0));
                });

                var salesYear = 0M;
                bills.ToList().GroupBy(x => new { x.ORDER_ID, x.COST, x.PERSON_NUM }).ToList().ForEach(x =>
                {
                    salesYear += x.Key.COST * (x.Key.PERSON_NUM ?? 0);
                    salesYear += x.Sum(c => c.PRICE * (c.QTY ?? 0));
                });

                var billsWeek = bills.Where(x => x.DATE >= DateTime.Today.AddDays(-7)).ToList();
                var week = billsWeek.GroupBy(x => x.DATE.Date)
                    .Select(x => new
                    {
                        DATE = x.Key,
                        SALES = x.ToList().Sum(c =>
                        {
                            var t = 0M;
                            x.ToList().GroupBy(d => new { d.ORDER_ID, d.COST, d.PERSON_NUM }).ToList()
                            .ForEach(v =>
                            {
                                t += v.Key.COST * (v.Key.PERSON_NUM ?? 0);
                                t += v.Sum(b => b.PRICE * (b.QTY ?? 0));
                            });
                            return t;
                        }) / x.Count()
                    }).OrderBy(x=>x.DATE).ToList();
                return new
                {
                    salesToday,
                    salesMonth,
                    salesHalfYear = salesYear,
                    week
                };

            }
        }

        

    }
}
