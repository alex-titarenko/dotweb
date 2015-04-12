using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace TAlex.Web.Mvc.Helpers
{
    public static class ImageExtensions
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string id, string src, string alternateText)
        {
            return Image(helper, id, src, alternateText, null);

        }

        public static MvcHtmlString Image(this HtmlHelper helper, string id, string src, string alternateText, object htmlAttributes)
        {
            return Image(helper, id, src, null, alternateText, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string id, string src, string srcSet, string alternateText, object htmlAttributes)
        {
            // Instantiate a UrlHelper 
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            // Create tag builder
            TagBuilder builder = new TagBuilder("img");

            // Create valid id
            builder.GenerateId(id);

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
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
