using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;

namespace TAlex.Web.Mvc.Helpers
{
    public static class ImageExtensions
    {
        public static HtmlString Image(this HtmlHelper helper, string id, string src, string alternateText)
        {
            return Image(helper, id, src, alternateText, null);

        }

        public static HtmlString Image(this HtmlHelper helper, string id, string src, string alternateText, object htmlAttributes)
        {
            return Image(helper, id, src, null, alternateText, htmlAttributes);
        }

        public static HtmlString Image(this HtmlHelper helper, string id, string src, string srcSet, string alternateText, object htmlAttributes)
        {
            // Instantiate a UrlHelper 
            var actionContext = new ActionContext(helper.ViewContext.HttpContext, helper.ViewContext.RouteData, helper.ViewContext.ActionDescriptor);
            UrlHelper urlHelper = new UrlHelper(actionContext);

            // Create tag builder
            TagBuilder builder = new TagBuilder("img")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };

            // Create valid id
            builder.GenerateId(id, "_");

            // Add attributes
            builder.MergeAttribute("src", urlHelper.Content(src));
            builder.MergeAttribute("alt", alternateText);
            if (!String.IsNullOrEmpty(srcSet))
            {
                builder.MergeAttribute("srcset", srcSet);
            }
            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }

            // Render tag
            return new HtmlString(builder.ToString());
        }
    }
}
