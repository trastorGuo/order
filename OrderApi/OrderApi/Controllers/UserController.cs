using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrderApi.Domains;
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
                    var name = jt["NAME"].ToString();
                    var address = jt["ADDRESS"].ToString();
                    var account = jt["ACCOUNT"].ToString();
                    var password = jt["PASSWORD"].ToString();
                    var tel = jt["TEL"].ToString();
                    var urls = JsonConvert.DeserializeObject<List<IMAGE>>(jt["URLS"].ToString());
                    if (LoginDomain.Current.UserIsExsist(name))
                    {
                        throw new Exception("名称已存在！");
                    }
                    var m = new SHOP();
                    m.ID = Guid.NewGuid().ToString("N").ToUpper();
                    m.UserCreated = ACCOUNT;
                    m.DatetimeCreated = DateTime.Now;
                    m.STATE = 'A';
                    m.ACCOUNT = account;
                    m.ADDRESS = address;
                    m.PASSWORD = password;
                    m.TEL = tel;
                    m.IsAdmin = "N";
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
                    var name = jt["NAME"].ToString();
                    var address = jt["ADDRESS"].ToString();
                    var account = jt["ACCOUNT"].ToString();
                    var password = jt["PASSWORD"].ToString();
                    var tel = jt["TEL"].ToString();
                    var urls = JsonConvert.DeserializeObject<List<IMAGE>>(jt["URLS"].ToString());
                    var m = (from p in db.Shops where p.ACCOUNT == account select p).FirstOrDefault();
                    m.UserModified = ACCOUNT;
                    m.DatetimeModified = DateTime.Now;
                    m.STATE = 'A';
                    m.ACCOUNT = account;
                    m.ADDRESS = address;
                    m.PASSWORD = password;
                    m.TEL = tel;
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
                var urls = (from p in db.Images where p.ConnectId == shopInfo.ID select p).Select(x => new
                {
                    ID = x.ID,
                    URL = x.URL
                }).ToList();
                var DeskList = (from p in db.ShopDesks where p.ShopId == SHOP_ID select p).Select(x=>new
                {
                    x.DescDesc,
                    x.DeskCount
                }).ToList();
                return new
                {
                    shopInfo.NAME,
                    shopInfo.ADDRESS,
                    shopInfo.ACCOUNT,
                    shopInfo.PASSWORD,
                    shopInfo.TEL,
                    URLS = urls,
                    DeskList
                };
            }
        }
    }
}
