using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OrderApi.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApi.MsgCommon
{
    public class QueryRequired : ActionFilterAttribute
    {
        private string request { get; set; }
        private string response { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (var item in context.ActionArguments) {
                var dt = new KeyValuePair<string, string>(item.Key.ToString(), item.Value.ToString());
                list.Add(dt);
            }
            request = JsonConvert.SerializeObject(list);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;
            if (context.Exception != null)
                response = context.Exception.Message;
            else response = JsonConvert.SerializeObject(result.Value);

            LogsDomain.Current.Add(request, response);
        }
    }
}
