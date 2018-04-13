using Microsoft.AspNetCore.Mvc.Rendering;


namespace TAlex.AspNetCore.Mvc.Extensions
{
    public static class FormExtensions
    {
        public static MvcForm BeginMultipartForm(this IHtmlHelper htmlHelper)
        {
            return BeginMultipartForm(htmlHelper, null, null, FormMethod.Post);
        }

        public static MvcForm BeginMultipartForm(this IHtmlHelper htmlHelper, object routeValues)
        {
            return BeginMultipartForm(htmlHelper, null, null, routeValues, FormMethod.Post);
        }

        public static MvcForm BeginMultipartForm(this IHtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method)
        {
            return BeginMultipartForm(htmlHelper, actionName, controllerName, htmlHelper.ViewContext.RouteData.Values.ToRouteValues(), method);
        }

        public static MvcForm BeginMultipartForm(this IHtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, FormMethod method)
        {
            return htmlHelper.BeginForm(actionName, controllerName, routeValues, method, false, new { enctype = "multipart/form-data" });
        }
    }
}
