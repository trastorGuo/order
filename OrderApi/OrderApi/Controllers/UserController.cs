using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            return AuthDomain.Current.BuildToken(name, shopInfo.IsAdmin);
        }


        [HttpPost]
        public void AddShop(Object value)
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
                    var m = JsonConvert.DeserializeObject<SHOP>(value.ToString());
                    if (LoginDomain.Current.UserIsExsist(m.NAME))
                    {
                        throw new Exception("名称已存在！");
                    }
                    m.ID = Guid.NewGuid().ToString("N").ToUpper();
                    m.UserCreated = ACCOUNT;
                    m.DatetimeCreated = DateTime.Now;
                    m.STATE = 'A';
                    db.Insert(m);
                    db.CommitTransaction();
                }
                catch(Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
            
        }

        [HttpPost]
        [Auth]
        public void EditShop(Object value)
        {
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var m = JsonConvert.DeserializeObject<SHOP>(value.ToString());
                    db.Update(m);
                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }

        }

        [HttpGet]
        [Auth]
        public object ShopInfo()
        {
            return LoginDomain.Current.ShopInfo(ACCOUNT);
        }
    }
}
