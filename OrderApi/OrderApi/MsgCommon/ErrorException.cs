using MsgCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderApi.MsgCommon
{
    public class ErrorException : Exception
    {
        public ErrorException(string errMsg, HttpStatusCode status) :base(errMsg)
        {
            CustomExceptionAttribute.status = status;
        }
        public ErrorException(string errMsg):base(errMsg)
        {
            CustomExceptionAttribute.status = HttpStatusCode.OK;
        }
    }
}
