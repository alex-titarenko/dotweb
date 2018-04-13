using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace TAlex.AspNetCore.Mvc.Extensions
{
    public static class UploadExtensions
    {
        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name)
        {
            return htmlHelper.Upload(name, (IDictionary<string, object>)null);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, string accept)
        {
            return htmlHelper.Upload(name, accept, (IDictionary<string, object>)null);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, UploadFile accept)
        {
            return htmlHelper.Upload(name, accept, (IDictionary<string, object>)null);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, string accept, bool multiple)
        {
            return htmlHelper.Upload(name, accept, multiple, (IDictionary<string, object>)null);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, UploadFile accept, bool multiple)
        {
            return htmlHelper.Upload(name, accept, multiple, (IDictionary<string, object>)null);
        }


        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(name, null, htmlAttributes);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return htmlHelper.Upload(name, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, string accept, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, false, htmlAttributes);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, string accept, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, UploadFile accept, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, false, htmlAttributes);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, UploadFile accept, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlContent Upload(this HtmlHelper htmlHelper, string name, UploadFile accept, bool multiple, IDictionary<string, object> htmlAttributes)
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

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, UploadFile accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, multiple, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, string accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            return UploadHelper(htmlHelper, name, accept, multiple, htmlAttributes);
        }

        public static IHtmlContent Upload(this IHtmlHelper htmlHelper, string name, string accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(name, accept, multiple, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression));
        }

        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string accept, bool multiple)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple);
        }

        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, UploadFile accept, bool multiple)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple);
        }

        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }

        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, UploadFile accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }

        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }

        public static IHtmlContent UploadFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, UploadFile accept, bool multiple, object htmlAttributes)
        {
            return htmlHelper.Upload(ExpressionHelper.GetExpressionText((LambdaExpression)expression), accept, multiple, htmlAttributes);
        }


        private static IHtmlContent UploadHelper(IHtmlHelper htmlHelper, string name, string accept, bool multiple, IDictionary<string, object> htmlAttributes)
        {
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            
            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {
                throw new ArgumentException("name");
            }

            TagBuilder tagBuilder = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };
            tagBuilder.MergeAttribute("type", "file");
            tagBuilder.GenerateId(fullHtmlFieldName, "_");
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

            ModelStateEntry state;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out state) && (state.Errors.Count > 0))
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }

            var expression = name;
            var modelExplorer = ExpressionMetadataProvider.FromStringExpression(expression, htmlHelper.ViewContext.ViewData, htmlHelper.MetadataProvider);
            var validator = (ValidationHtmlAttributeProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(ValidationHtmlAttributeProvider));
            validator?.AddAndTrackValidationAttributes(htmlHelper.ViewContext, modelExplorer, expression, tagBuilder.Attributes);

            return new HtmlString(tagBuilder.ToString());
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
