using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

namespace TAlex.Web.Mvc.UI.Pager
{
    public class Pager : IHtmlContent
    {
        #region Fields

        private readonly int _pageNumber;
        private readonly int _totalPages;
        private readonly int _totalItems;
        private readonly ViewContext _viewContext;


        private int _displayedPages = 9;             // The maximum number of page numbers to display.

        protected string _cssClass = "reset pagination";
        protected string _containerTagName = "ul";

        protected string _navigationPagerItemWrapperFormat = "<li class=\"inline-block {1}\">{0}</li>";
        protected string _numPagerItemWrapperFormat = "<li class=\"inline-block\">{0}</li>";
        protected string _currentPageItemWrapperFormat = "<li class=\"inline-block active\"><a href=\"#\">{0}</a></li>";

        protected string _paginationFirst = "<span class='first'></span>";
        protected string _paginationPrev = "<span class='prev'></span>";
        protected string _paginationNext = "<span class='next'></span>";
        protected string _paginationLast = "<span class='last'></span>";

        protected string _pageQueryName = "page";

        protected Func<int, string> _urlBuilder;

        #endregion

        #region Properties

        protected ViewContext ViewContext
        {
            get { return _viewContext; }
        }

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
            //_pagination = pagination;
            _viewContext = context;

            _pageNumber = pageNumber;
            _totalPages = totalPages;
            _totalItems = totalItems;

            _urlBuilder = CreateDefaultUrl;
        }

        #endregion

        #region Methods

        // For backwards compatibility with WebFormViewEngine
        public override string ToString()
        {
            return ToHtmlString();
        }

        #region Fluent Interface

        /// <summary>
        /// Specifies the query string parameter to use when generating pager links. The default is 'page'
        /// </summary>
        public Pager QueryParam(string queryStringParam)
        {
            _pageQueryName = queryStringParam;
            return this;
        }

        public Pager DisplayedPages(int count)
        {
            _displayedPages = count;
            return this;
        }

        public Pager CssClass(string @class)
        {
            _cssClass = @class;
            return this;
        }

        public Pager ContainerTagName(string tagName)
        {
            _containerTagName = tagName;
            return this;
        }

        public Pager NavigationPagerItemWrapperFormat(string format)
        {
            _navigationPagerItemWrapperFormat = format;
            return this;
        }

        public Pager NumericPagerItemWrapperFormat(string format)
        {
            _numPagerItemWrapperFormat = format;
            return this;
        }

        public Pager CurrentPageItemWrapperFormat(string format)
        {
            _currentPageItemWrapperFormat = format;
            return this;
        }

        /// <summary>
        /// Text for the 'first' link.
        /// </summary>
        public Pager First(string first)
        {
            _paginationFirst = first;
            return this;
        }

        /// <summary>
        /// Text for the 'prev' link
        /// </summary>
        public Pager Previous(string previous)
        {
            _paginationPrev = previous;
            return this;
        }

        /// <summary>
        /// Text for the 'next' link
        /// </summary>
        public Pager Next(string next)
        {
            _paginationNext = next;
            return this;
        }

        /// <summary>
        /// Text for the 'last' link
        /// </summary>
        public Pager Last(string last)
        {
            _paginationLast = last;
            return this;
        }

        /// <summary>
        /// Uses a lambda expression to generate the URL for the page links.
        /// </summary>
        /// <param name="urlBuilder">Lambda expression for generating the URL used in the page links</param>
        public Pager Link(Func<int, string> urlBuilder)
        {
            _urlBuilder = urlBuilder;
            return this;
        }

        #endregion

        #region IHtmlString Members

        public string ToHtmlString()
        {
            if (_totalItems == 0)
            {
                return null;
            }

            var builder = new StringBuilder();
            var writer = new StringWriter(builder);

            if (_totalPages > 1)
            {
                WriteTo(writer, null);
            }

            return builder.ToString();
        }

        #endregion

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(CreateTag(_containerTagName, _cssClass));

            builder.Append(CreateNavigationPager(1, "firstpage", _paginationFirst, _pageNumber > 1));
            builder.Append(CreateNavigationPager(_pageNumber - 1, "prevpage", _paginationPrev, _pageNumber > 1));


            int firstVisiblePage = Math.Max(1, _pageNumber - _displayedPages / 2);
            int lastVisiblePage = Math.Min(_totalPages, _pageNumber + _displayedPages / 2);

            int correctedPagerItemCount = Math.Min(_displayedPages, _totalPages);
            if (lastVisiblePage - firstVisiblePage + 1 < correctedPagerItemCount)
            {
                if (firstVisiblePage == 1)
                {
                    lastVisiblePage = correctedPagerItemCount;
                }
                else if (lastVisiblePage == _totalPages)
                {
                    firstVisiblePage = lastVisiblePage - correctedPagerItemCount + 1;
                }
            }

            if (firstVisiblePage != 1) builder.Append(CreateNumericPager(firstVisiblePage - 1, "..."));

            for (int i = firstVisiblePage; i <= lastVisiblePage; i++)
            {
                builder.Append(CreateNumericPager(i));
            }

            if (lastVisiblePage != _totalPages) builder.Append(CreateNumericPager(lastVisiblePage + 1, "..."));


            builder.Append(CreateNavigationPager(_pageNumber + 1, "nextpage", _paginationNext, _pageNumber < _totalPages));


            int lastPage = _totalPages;

            builder.Append(CreateNavigationPager(lastPage, "lastpage", _paginationLast, _pageNumber < lastPage));

            builder.Append(CreateClosedTag(_containerTagName));

            writer.WriteLine(builder.ToString());
        }

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

            result = String.Format(_navigationPagerItemWrapperFormat, result, navName);

            return result;
        }

        private string CreateNumericPager(int pageNumber, string text = null)
        {
            if (pageNumber == _pageNumber)
            {
                return String.Format(_currentPageItemWrapperFormat, pageNumber);
            }
            else
            {
                if (String.IsNullOrEmpty(text)) text = pageNumber.ToString();
                return String.Format(_numPagerItemWrapperFormat, CreatePageLink(pageNumber, text));
            }
        }

        private string CreatePageLink(int pageNumber, string text)
        {
            return String.Format("<a href=\"{0}\">{1}</a>", _urlBuilder(pageNumber), text);
        }

        private string CreatePageDisabledLink(string text)
        {
            return String.Format("<a disabled=\"disabled\">{0}</a>", text);
        }

        private string CreateDefaultUrl(int pageNumber)
        {
            var query = _viewContext.HttpContext.Request.QueryString;
            query.Add(_pageQueryName, pageNumber.ToString());

            var queryPart = query.ToUriComponent();

            var path = _viewContext.HttpContext.Request.Path.ToUriComponent();

            return '/' + path.TrimStart('/') + queryPart;
        }

        #endregion

        #endregion
    }
}
