using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.MsgCommon
{
    public class AuthCommon
    {
        private static AuthCommon _current;
        public static AuthCommon Current = _current ?? new AuthCommon();
        public string BuildToken(string userId)
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
