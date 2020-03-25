using CoreBase.Api.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreBase.Api
{
    public class BaseApiController : ControllerBase
    {
        protected ObjectResult StatusCode(HttpStatusCode statusCode, object value) =>
            StatusCode((int)statusCode, value);
        protected ObjectResult GeneralResponse(object obj, IEnumerable<string> errors = null)
        {
            var generalBaseResponse = obj as GeneralBaseResponse;
            var data = generalBaseResponse.GetType().GetProperty("Data");
            if (data != null)
                return StatusCode(generalBaseResponse.Result.HttpStatus, ApiResult<object>.Result(generalBaseResponse.Result,  data.GetValue(obj)));
            else
                return StatusCode(generalBaseResponse.Result.HttpStatus, ApiResult.Result(generalBaseResponse.Result,   errors));

        }
    }
}
