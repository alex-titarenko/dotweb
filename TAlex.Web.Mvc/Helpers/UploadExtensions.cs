using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace TAlex.Web.Mvc.Helpers
{
    public static class UploadExtensions
    {
        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name)
        {
            return htmlHelper.Upload(name, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, string accept)
        {
            return htmlHelper.Upload(name, accept, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, UploadFile accept)
        {
            return htmlHelper.Upload(name, accept, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, string accept, bool multiple)
        {
            return htmlHelper.Upload(name, accept, multiple, (IDictionary<string, object>)null);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, UploadFile accept, bool multiple)
        {
            return htmlHelper.Upload(name, accept, multiple, (IDictionary<string, object>)null);
        }


        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(name, null, htmlAttributes);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return htmlHelper.Upload(name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, string accept, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, false, htmlAttributes);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, string accept, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, UploadFile accept, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, false, htmlAttributes);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, UploadFile accept, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, UploadFile accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            List<string> acceptValues = new List<string>();
            // TODO: Need to refactoring.
            //if (!accept.HasFlag(UploadFile.Any))
            //{
            //    if (accept.HasFlag(UploadFile.Audio))
            //        acceptValues.Add("audio/*");
            //    if (accept.HasFlag(UploadFile.Video))
            //        acceptValues.Add("video/*");
            //    if (accept.HasFlag(UploadFile.Image))
            //        acceptValues.Add("image/*");
            //}

            return htmlHelper.Upload(name, String.Join(",", acceptValues), multiple, htmlAttributes);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, UploadFile accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, multiple, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, string accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            return UploadHelper(htmlHelper, name, accept, multiple, htmlAttributes);
        }

        public static MvcHtmlString Upload(this HtmlHelper htmlHelper, string name, string accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, multiple, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression));
        }

        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string accept, bool multiple)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple);
        }

        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, UploadFile accept, bool multiple)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple);
        }

        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }

        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, UploadFile accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }

        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }

        public static MvcHtmlString UploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, UploadFile accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }


        private static MvcHtmlString UploadHelper(HtmlHelper htmlHelper, string name, string accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            
            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {
                throw new ArgumentException("name");
            }
            
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("type", "file");
            tagBuilder.GenerateId(fullHtmlFieldName);
            tagBuilder.MergeAttribute("name", fullHtmlFieldName, true);
            tagBuilder.MergeAttributes<string, object>(htmlAttributes, true);
            if (!String.IsNullOrEmpty(accept))
            {
                tagBuilder.MergeAttribute("accept", accept);
            }
            if (multiple)
            {
                tagBuilder.MergeAttribute("multiple", null);
            }

            ModelState state;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out state) && (state.Errors.Count > 0))
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            tagBuilder.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name));

            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }

    [Flags]
    public enum UploadFile
    {
        Audio = 1,
        Video = 2,
        Image = 4,
        Any = 256,
    }
}
