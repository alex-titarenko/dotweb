using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace TAlex.Web.Mvc.Helpers
{
    public static class FormExtensions
    {
        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper)
        {
            return BeginMultipartForm(htmlHelper, null, null, FormMethod.Post);
        }

        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper, object routeValues)
        {
            return BeginMultipartForm(htmlHelper, null, null, routeValues, FormMethod.Post);
        }

        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method)
        {
            return BeginMultipartForm(htmlHelper, actionName, controllerName, htmlHelper.ViewContext.RouteData.Values.ToRouteValues(), method);
        }

        public static MvcForm BeginMultipartForm(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, FormMethod method)
        {
            return htmlHelper.BeginForm(actionName, controllerName, routeValues, method, false, new { enctype = "multipart/form-data" });
        }
    }
}
