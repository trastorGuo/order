using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OrderApi.Domains;
using OrderApi.MsgCommon;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [WebApi]
    public class AuthController : BaseController
    {
        [HttpGet]
        public string GetToken(string name, string pwd)
        {
            //校验用户
            if (!LoginDomain.Current.CheckPassword(name, pwd))
            {
                throw new Exception("当前用户名或密码不正确！");
            }
            var shopInfo = LoginDomain.Current.ShopInfo(name);
            //返回Token
            return AuthDomain.Current.BuildToken(name, shopInfo.IsAdmin);
        }

    }
}
