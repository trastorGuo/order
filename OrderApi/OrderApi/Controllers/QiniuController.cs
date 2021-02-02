using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.MsgCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Controllers
{
    [WebApi]
    public class QiniuController : BaseController
    {
        QiniuConfig mac = new QiniuConfig("ClIOkNHZ5aCGStAfuRj4fm_8trdXawNcbwn6-s-X", "Li93J1rRImEgs6R_3v44Cx0uxSTA0lRvYz-_qnEl");


        [HttpGet]
        public object GetToken()
        {
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = "trastor";
            putPolicy.SetExpires(3600);
            return SignWithData(putPolicy.ToJsonString());
        }

        private string SignWithData(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return SignWithData(data);
        }

        private string SignWithData(byte[] data)
        {
            string sstr = Base64.UrlSafeBase64Encode(data);
            return string.Format("{0}:{1}:{2}", mac.AccessKey, encodedSign(sstr), sstr);
        }

        private string encodedSign(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return encodedSign(data);
        }

        private string encodedSign(byte[] data)
        {
            HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(mac.SecretKey));
            byte[] digest = hmac.ComputeHash(data);
            return Base64.UrlSafeBase64Encode(digest);
        }
    }
}
