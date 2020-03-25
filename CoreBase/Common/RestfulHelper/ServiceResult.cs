using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreBase.Common.RestfulHelper
{
    public class ServiceResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string Data { get; set; }
        public Exception Error { get; set; }
    }

    public class ServiceResult<TResult>
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string ReasonPhrase { get; set; }
        public TResult Data { get; set; }
        public string Error { get; set; }
    }
}
