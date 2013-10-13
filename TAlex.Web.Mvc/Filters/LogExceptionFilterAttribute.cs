using System;
using System.Diagnostics;
using System.Web.Mvc;


namespace TAlex.Web.Mvc.Filters
{
    public class LogExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public string SourceName { get; set; }


        public LogExceptionFilterAttribute(string sourceName)
        {
            SourceName = sourceName;
        }


        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled) return;

            TraceSource traceSource = new TraceSource(SourceName, SourceLevels.All);
            traceSource.TraceData(TraceEventType.Error, 1, filterContext.Exception);
        }
    }
}
