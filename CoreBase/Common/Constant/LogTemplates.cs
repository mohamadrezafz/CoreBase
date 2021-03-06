﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBase.Common.Constant
{
    public class LogTemplates
    {
        public const string Request = "Request Url: {@Url},Body: {@Body},Header: {@Header}";
        public const string Response = "Response Url: {@Url},HttpStatus: {@HttpStatus},Body: {@Body}";
        public const string Exception = "Exception";
        public const string Information = "Information ClassName: {@ClassName}, MethodName: {@MethodName}";
        public const string RequestSoap = "RequestSoap  Request: {@Request}";
        public const string ResponseSoap = "RequestSoap Response: {@Response}";
    }
}
