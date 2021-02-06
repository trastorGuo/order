using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Domains;
using OrderApi.MsgCommon;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace OrderApi.Controllers
{
    [WebApi]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PrinterController : BaseController
    {

        
        [HttpGet]
        [Auth]
        public bool AddPrinter(string snslist)
        {
            PrinterDomain.Current.addprinter(snslist);
            return true;
        }


        [HttpGet]
        public bool ClearPrintStatus(string account)
        {
            PrinterDomain.Current.clearPrintStatus(account);
            return true;
        }


        //public bool Print()
        //{
        //    PrinterDomain.Current.print()
        //}

    }
}
