using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Http;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("order/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FoodController : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> Get()
        {
            using (var db = new OrderDB())
            {
                var query = from p in db.Shops select p;
                return query.ToList();
            }
        }

        [HttpGet]
        public ActionResult<int> listById(int i)
        {
            return i + 1;
        }
    }
}
