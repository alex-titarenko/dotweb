using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;


namespace TAlex.Web.Mvc.Helpers
{
    public static class FormExtensions
    {
        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper)
        {
            return BeginMultipartForm(htmlHelper, null, null, FormMethod.Post);
        }

        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper, RouteValueDictionary routeValues)
        {
            return BeginMultipartForm(htmlHelper, null, null, routeValues, FormMethod.Post);
        }

        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method)
        {
            return BeginMultipartForm(htmlHelper, actionName, controllerName, htmlHelper.ViewContext.HttpContext.Request.QueryString.ToRouteValues(), method);
        }

        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper, string actionName, string controllerName, RouteValueDictionary routeValues, FormMethod method)
        {
            return htmlHelper.BeginForm(actionName, controllerName, routeValues, method, new Dictionary<string, object>() { { "enctype", "multipart/form-data" } });
        }
    }
}
