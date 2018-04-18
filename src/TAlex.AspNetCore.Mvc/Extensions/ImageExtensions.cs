using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Text.Encodings.Web;

namespace TAlex.AspNetCore.Mvc.Extensions
{
    public static class ImageExtensions
    {
        public static IHtmlContent Image(this IHtmlHelper helper, string id, string src, string alternateText)
        {
            return Image(helper, id, src, alternateText, null);

        }

        public static IHtmlContent Image(this IHtmlHelper helper, string id, string src, string alternateText, object htmlAttributes)
        {
            return Image(helper, id, src, null, alternateText, htmlAttributes);
        }

        public static IHtmlContent Image(this IHtmlHelper helper, string id, string src, string srcSet, string alternateText, object htmlAttributes)
        {
            // Instantiate a UrlHelper 
            var actionContext = new ActionContext(helper.ViewContext.HttpContext, helper.ViewContext.RouteData, helper.ViewContext.ActionDescriptor);
            UrlHelper urlHelper = new UrlHelper(actionContext);

            // Create tag builder
            var builder = new TagBuilder("img")
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
            using (var writer = new StringWriter())
            {
                builder.WriteTo(writer, HtmlEncoder.Default);
                var htmlOutput = writer.ToString();
                return new HtmlString(htmlOutput);
            }
        }
    }
}
