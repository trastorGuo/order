using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Domains
{
    public class LoginDomain
    {
        private static LoginDomain _current;
        public static LoginDomain Current = _current ?? new LoginDomain();

        public bool CheckPassword(string name, string pwd)
        {
            using(var db = new OrderDB())
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception($"用户名不能为空！");
                }
                if (string.IsNullOrEmpty(pwd))
                {
                    throw new Exception($"密码不能为空！");
                }
                var isExsit = from p in db.Shops
                              where p.ACCOUNT == name && p.PASSWORD == pwd
                              select p;
                return isExsit.Count() > 0;
            }
        }

        public SHOP ShopInfo(string account)
        {
            using (var db = new OrderDB())
            {
                var infos = from p in db.Shops
                            where p.ACCOUNT.Contains(account)
                            select p;
                var info = infos.FirstOrDefault();
                if (info is null)
                {
                    throw new Exception("当前店铺不存在！");
                }
                return info;
            }

        }

        public bool UserIsExsist(string name)
        {
            using (var db = new OrderDB())
            {
                var isExsit = from p in db.Shops
                              where p.ACCOUNT == name
                              select p;
                return isExsit.Count() > 0;
            }
        }
    }
}
