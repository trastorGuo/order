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
    public class UserController : Controller
    {
        
        /// <summary>
        /// 登录校验成功返回token
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet("name={name}&pwd={pwd}")]
        public string Login(string name, string pwd)
        {
            //校验用户
            if (!LoginDomain.Current.CheckPassword(name, pwd))
            {
                throw new Exception("当前用户名或密码不正确！");
            }
            //返回Token
            return AuthCommon.Current.BuildToken(name);
        }


        [HttpPost]
        public void AddShop([FromBody] Object value)
        {
            using (var db = new OrderDB())
            {
                db.BeginTransaction();
                try
                {
                    var m = JsonConvert.DeserializeObject<SHOP>(value.ToString());
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
        public void EditShop([FromBody] Object value)
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

        [HttpGet("name={name}")]
        public object ShopInfo(string name)
        {
            using (var db = new OrderDB())
            {
                var infos = from p in db.Shops
                           where p.NAME.Contains(name)
                           select p;
                var info = infos.FirstOrDefault();
                if(info is null)
                {
                    throw new Exception("当前店铺不存在！");
                }
                return info;
            }

        }
    }
}
