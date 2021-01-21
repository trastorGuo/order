﻿using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Domains
{
    public class AuthDomain
    {
        private static AuthDomain _current;
        public static AuthDomain Current = _current ?? new AuthDomain();
        public string BuildToken(string userId, string isAdmin, string shopId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Security:Tokens:Key");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Security:Tokens:Issuer",
                Audience = "Security:Tokens:Audience",
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim(ClaimTypes.Name, userId),
                    new Claim(ClaimTypes.Role, isAdmin),
                    new Claim("SHOP_ID", shopId)
                }),
                Expires = DateTime.UtcNow.AddHours(2.5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            var t1 = tokenHandler.WriteToken(token);
            return t1;
        }
    }
}
