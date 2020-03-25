using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreBase.Api.Response
{
    public class HttpBodyResponse
    {
        public virtual string Message { get; set; }
        public virtual string Code { get; set; }
        public virtual HttpStatusCode HttpStatus { get; set; }

        public HttpBodyResponse(string message, string code,  HttpStatusCode httpStatus)
        {
            this.Message = message;
            this.Code = code;
            this.HttpStatus = httpStatus;
        }
        public static HttpBodyResponse Ok => new HttpBodyResponse("انجام عملیات موفق آمیز بود", "00000", HttpStatusCode.OK);
        public static HttpBodyResponse Created => new HttpBodyResponse("انجام عملیات موفق آمیز بود", "00000", HttpStatusCode.Created);
        public static HttpBodyResponse Error => new HttpBodyResponse("خطای سیستمی رخ داده است", "00500", HttpStatusCode.InternalServerError);
        public static HttpBodyResponse BadRequest => new HttpBodyResponse("خطا در مقادیر ورودی", "00400", HttpStatusCode.BadRequest);
        public static HttpBodyResponse NotFound => new HttpBodyResponse("داده ای برای اعمال یافت نشد.", "00404", HttpStatusCode.NotFound);


    }

}
