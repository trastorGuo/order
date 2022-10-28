using DataModels;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.Domains
{
    public class LogsDomain
    {
        private static LogsDomain _current;
        public static LogsDomain Current = _current ?? new LogsDomain();

        public void Add(string action, string request, string response, string user = "SYS")
        {
            using(var db = new OrderDB())
            {
                var info = new LOG();
                info.ID = Guid.NewGuid().ToString("N").ToUpper();
                info.DatetimeCreated = DateTime.Now;
                info.UserCreated = user;
                info.STATE = 'A';
                info.ACTION = action;
                info.REQUEST = request;
                info.RESPONSE = response;
                db.Insert(info);
            }
        }
    }
}
