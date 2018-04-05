using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Diagnostics;


namespace TAlex.Web.Mvc.Filters
{
    public class LogExceptionFilterAttribute : ActionFilterAttribute, IExceptionFilter
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
