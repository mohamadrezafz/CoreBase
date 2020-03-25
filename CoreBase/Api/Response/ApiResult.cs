using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBase.Api.Response
{
  public  class ApiResult
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public static ApiResult Result(HttpBodyResponse result, IEnumerable<string> errorMessages = null) => new ApiResult()
        {
            Code = result.Code,
            Message = result.Message,
            ErrorMessages = errorMessages
        };

    }


    public class ApiResult<T> : ApiResult
    {
        public T Data { get; set; }
        public static ApiResult<T> Result(HttpBodyResponse result,T obj, IEnumerable<string> errorMessages = null) => new ApiResult<T>
        {
            Code = result.Code,
            Message = result.Message,
            Data = obj,
            ErrorMessages = errorMessages
        };
    }
}
