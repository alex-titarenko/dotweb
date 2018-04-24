using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace TAlex.AspNetCore.Mvc.UI.Pager
{
    public class Pager : IHtmlContent
    {
        #region Fields

        private readonly int pageNumber;
        private readonly int totalPages;
        private readonly int totalItems;
        private int displayedPages = 9;             // The maximum number of page numbers to display.

        protected string cssClass = "reset pagination";
        protected string containerTagName = "ul";

        protected string navigationPagerItemWrapperFormat = "<li class=\"inline-block {1}\">{0}</li>";
        protected string numPagerItemWrapperFormat = "<li class=\"inline-block\">{0}</li>";
        protected string currentPageItemWrapperFormat = "<li class=\"inline-block active\"><a href=\"#\">{0}</a></li>";

        protected string paginationFirst = "<span class='first'></span>";
        protected string paginationPrev = "<span class='prev'></span>";
        protected string paginationNext = "<span class='next'></span>";
        protected string paginationLast = "<span class='last'></span>";

        protected string pageQueryName = "page";

        protected Func<int, string> urlBuilder;

        #endregion

        #region Properties

        protected ViewContext ViewContext { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Pager class.
        /// </summary>
        /// <param name="context">The view context</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="totalPages">The total number of pages.</param>
        /// <param name="totalItems">The number of items.</param>
        public Pager(ViewContext context, int pageNumber, int totalPages, int totalItems)
        {
            this.ViewContext = context;

            this.pageNumber = pageNumber;
            this.totalPages = totalPages;
            this.totalItems = totalItems;

            this.urlBuilder = CreateDefaultUrl;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the string representation of Pager object.
        /// </summary>
        /// <returns>string of Pager object.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
            {
                this.WriteTo(writer, HtmlEncoder.Default);
                var result = builder.ToString();
                return result;
            }
        }

        #region Fluent Interface

        /// <summary>
        /// Specifies the query string parameter to use when generating pager links. The default is 'page'
        /// </summary>
        public Pager QueryParam(string queryStringParam)
        {
            this.pageQueryName = queryStringParam;
            return this;
        }

        public Pager DisplayedPages(int count)
        {
            this.displayedPages = count;
            return this;
        }

        public Pager CssClass(string @class)
        {
            this.cssClass = @class;
            return this;
        }

        public Pager ContainerTagName(string tagName)
        {
            this.containerTagName = tagName;
            return this;
        }

        public Pager NavigationPagerItemWrapperFormat(string format)
        {
            this.navigationPagerItemWrapperFormat = format;
            return this;
        }

        public Pager NumericPagerItemWrapperFormat(string format)
        {
            this.numPagerItemWrapperFormat = format;
            return this;
        }

        public Pager CurrentPageItemWrapperFormat(string format)
        {
            this.currentPageItemWrapperFormat = format;
            return this;
        }

        /// <summary>
        /// Text for the 'first' link.
        /// </summary>
        public Pager First(string first)
        {
            this.paginationFirst = first;
            return this;
        }

        /// <summary>
        /// Text for the 'prev' link
        /// </summary>
        public Pager Previous(string previous)
        {
            this.paginationPrev = previous;
            return this;
        }

        /// <summary>
        /// Text for the 'next' link
        /// </summary>
        public Pager Next(string next)
        {
            this.paginationNext = next;
            return this;
        }

        /// <summary>
        /// Text for the 'last' link
        /// </summary>
        public Pager Last(string last)
        {
            this.paginationLast = last;
            return this;
        }

        /// <summary>
        /// Uses a lambda expression to generate the URL for the page links.
        /// </summary>
        /// <param name="urlBuilder">Lambda expression for generating the URL used in the page links</param>
        public Pager Link(Func<int, string> urlBuilder)
        {
            this.urlBuilder = urlBuilder;
            return this;
        }

        #endregion

        #region IHtmlContent Members

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            if (this.totalItems == 0 || this.totalPages <= 1)
            {
                return;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(CreateTag(this.containerTagName, this.cssClass));

            builder.Append(CreateNavigationPager(1, "firstpage", this.paginationFirst, this.pageNumber > 1));
            builder.Append(CreateNavigationPager(this.pageNumber - 1, "prevpage", this.paginationPrev, this.pageNumber > 1));


            int firstVisiblePage = Math.Max(1, this.pageNumber - this.displayedPages / 2);
            int lastVisiblePage = Math.Min(this.totalPages, this.pageNumber + this.displayedPages / 2);

            int correctedPagerItemCount = Math.Min(this.displayedPages, this.totalPages);
            if (lastVisiblePage - firstVisiblePage + 1 < correctedPagerItemCount)
            {
                if (firstVisiblePage == 1)
                {
                    lastVisiblePage = correctedPagerItemCount;
                }
                else if (lastVisiblePage == this.totalPages)
                {
                    firstVisiblePage = lastVisiblePage - correctedPagerItemCount + 1;
                }
            }

            if (firstVisiblePage != 1) builder.Append(CreateNumericPager(firstVisiblePage - 1, "..."));

            for (int i = firstVisiblePage; i <= lastVisiblePage; i++)
            {
                builder.Append(CreateNumericPager(i));
            }

            if (lastVisiblePage != this.totalPages) builder.Append(CreateNumericPager(lastVisiblePage + 1, "..."));


            builder.Append(CreateNavigationPager(this.pageNumber + 1, "nextpage", this.paginationNext, this.pageNumber < this.totalPages));


            int lastPage = this.totalPages;

            builder.Append(CreateNavigationPager(lastPage, "lastpage", this.paginationLast, this.pageNumber < lastPage));

            builder.Append(CreateClosedTag(this.containerTagName));

            writer.WriteLine(builder.ToString());
        }

        #endregion

        #region Helpers

        private static string CreateTag(string tagName, string @class = null)
        {
            if (String.IsNullOrEmpty(@class))
                return String.Format("<{0}>", tagName);
            else
                return String.Format("<{0} class=\"{1}\">", tagName, @class);
        }

        private static string CreateClosedTag(string tagName)
        {
            return String.Format("</{0}>", tagName);
        }


        private string CreateNavigationPager(int pageNumber, string navName, string innerText, bool enabled = true)
        {
            string result = String.Empty;

            if (enabled)
                result = CreatePageLink(pageNumber, innerText);
            else
                result = CreatePageDisabledLink(innerText);

            result = String.Format(this.navigationPagerItemWrapperFormat, result, navName);

            return result;
        }

        private string CreateNumericPager(int pageNumber, string text = null)
        {
            if (pageNumber == this.pageNumber)
            {
                return String.Format(this.currentPageItemWrapperFormat, pageNumber);
            }
            else
            {
                if (String.IsNullOrEmpty(text)) text = pageNumber.ToString();
                return String.Format(this.numPagerItemWrapperFormat, CreatePageLink(pageNumber, text));
            }
        }

        private string CreatePageLink(int pageNumber, string text)
        {
            return String.Format("<a href=\"{0}\">{1}</a>", this.urlBuilder(pageNumber), text);
        }

        private string CreatePageDisabledLink(string text)
        {
            return String.Format("<a disabled=\"disabled\">{0}</a>", text);
        }

        private string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new Dictionary<string, StringValues>
            {
                { this.pageQueryName, pageNumber.ToString() }
            };

            //Re-add existing querystring
            foreach (var key in ViewContext.HttpContext.Request.Query.Keys.Where(key => key != null))
            {
                if (!routeValues.ContainsKey(key))
                {
                    routeValues[key] = ViewContext.HttpContext.Request.Query[key];
                }
            }

            var queryPart = QueryString.Create(routeValues);
            var path = ViewContext.HttpContext.Request.Path.ToUriComponent();

            return '/' + path.TrimStart('/') + queryPart.ToUriComponent();
        }

        #endregion

        #endregion
    }
}
