using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CoreBase.Api.Response
{
    public class GeneralBaseResponse<T> : GeneralBaseResponse
    {
        public T Data { get; set; }

    }
    public class GeneralBaseResponse
    {
        public HttpBodyResponse Result { get; set; }

    }
   
}
