using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.MsgCommon
{
    public class TokenFilter : ActionFilterAttribute
    {
        public TokenFilter() //通过依赖注入得到数据访问层实例
        {
            //tokenHelper = _tokenHelper;
        }
       

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token)) return;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tt = tokenHandler.ReadJwtToken(token).Claims.ToList();
            BaseController.ACCOUNT = tt[0].Value;
            BaseController.IS_ADMIN = tt[1].Value == "Y";
            BaseController.TOKEN = token;
            BaseController.SHOP_ID = tt[2].Value;
        }
    }
}
