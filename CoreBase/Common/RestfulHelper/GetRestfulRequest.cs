using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBase.Common.RestfulHelper
{
    public class GetRestfulRequest
    {
        public GetRestfulRequest()
        {
            Parameters = null;
            Headers = null;
            BypassCertificate = false;
            Timeout = new TimeSpan();
        }
        public string Url { get; set; }
        public List<KeyValuePair<string, string>> Parameters { get; set; }
        public List<KeyValuePair<string, string>> Headers { get; set; }
        public bool BypassCertificate { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}
