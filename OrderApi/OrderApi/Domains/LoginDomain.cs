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
                              where p.ACCOUNT == name && p.PASSWORD == Convert.ToDecimal(pwd)
                              select p;
                return isExsit.Count() > 0;
            }
        }
    }
}
