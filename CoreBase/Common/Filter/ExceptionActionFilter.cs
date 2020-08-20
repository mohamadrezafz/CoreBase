using CoreBase.Api.Response;
using CoreBase.Common.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBase.Common.Filter
{
    public class ExceptionActionFilter : ExceptionFilterAttribute
    {
        public ExceptionActionFilter()
        {
        }

        #region Overrides of ExceptionFilterAttribute

        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) return;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)HttpBodyResponse.Error.HttpStatus;

           // var userData = UserData.GetUserData(context.HttpContext.Request);

            Log.Error(
                context.Exception,
                LogTemplates.Exception
                );
    
            var obj = ApiResult.Result(HttpBodyResponse.Error);

            context.Result = new JsonResult(obj);
            context.ExceptionHandled = true;
            base.OnException(context);
        }


        #endregion
    }
}
