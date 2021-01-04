using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace MsgCommon
{
    public class WebApiResultMiddleware : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //根据实际需求进行具体实现
            if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;
                if (objectResult.Value == null)
                {
                    context.Result = new ObjectResult(new SuccessResultModel (404,  "未找到资源",  true ));
                }
                else
                {
                    context.Result = new ObjectResult(new SuccessResultModel(200, objectResult.Value, true));
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new SuccessResultModel(404, "未找到资源", true));
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(new SuccessResultModel(200, (context.Result as ContentResult).Content, true));
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new SuccessResultModel((context.Result as StatusCodeResult).StatusCode, null, true));
            }
        }



    }

    public class SuccessResultModel
    {
        public SuccessResultModel(int? code = null, /*string message = null,*/
            object result = null, bool success = false)
        {
            this.success = success;
            this.code = code;
            this.data = result;
            //this.message.content = message;
        }
        public bool success { get; set; }
        public int? code { get; set; }
        public object data { get; set; }
        //public Message message { get; set; } = new Message();
    }
}