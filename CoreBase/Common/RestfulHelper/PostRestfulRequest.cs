using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBase.Common.RestfulHelper
{
    public class PostRestfulRequest : GetRestfulRequest
    {
        public PostRestfulRequest() : base()
        {
            Content = null;
        }
        public InputFormat Format { get; set; }
        public dynamic Content { get; set; }
    }

}
