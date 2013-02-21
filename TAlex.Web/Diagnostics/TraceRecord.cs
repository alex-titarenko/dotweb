using System;
using TAlex.Common.Diagnostics;


namespace TAlex.Web.Diagnostics
{
    public class TraceRecord
    {
        public string TraceIdentifier { get; set; }

        public string Description { get; set; }

        public string RequestUrl { get; set; }

        public string UserAgent { get; set; }

        public string UrlReferrer { get; set; }

        public string Handler { get; set; }

        public string HttpMethod { get; set; }

        public string PostData { get; set; }

        public string Status { get; set; }

        public string UserName { get; set; }

        public string UserHostAddress { get; set; }

        public string Event { get; set; }

        public ExceptionInfo Exception { get; set; }


        public TraceRecord()
        {
            TraceIdentifier = "TAlex.Web.Diagnostics.WebRequestTraceListener";
        }
    }
}
