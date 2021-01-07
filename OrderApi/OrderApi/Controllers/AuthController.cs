using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("common/[controller]/[action]")]
    public class AuthController : Controller
    {
        [HttpGet("name={name}&pwd={pwd}")]
        public string GetToken(string name, string pwd)
        {
            //校验用户

            //返回Token
            return BuildToken(name);
        }


        private string BuildToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Security:Tokens:Key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Security:Tokens:Issuer",
                Audience = "Security:Tokens:Audience",
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
