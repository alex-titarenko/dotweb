using Microsoft.AspNetCore.Mvc.Rendering;
using TAlex.AspNetCore.Mvc.Pagination;


namespace TAlex.AspNetCore.Mvc.UI.Pager
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Creates a pager component using the specified IPagination as the datasource.
        /// </summary>
        /// <param name="helper">The HTML Helper</param>
        /// <param name="pagination">The datasource</param>
        /// <returns>A Pager component</returns>
        public static Pager Pager(this IHtmlHelper helper, IPagination pagination)
        {
            return new Pager(helper.ViewContext, pagination.PageNumber, pagination.TotalPages, pagination.TotalItems);
        }
    }
}
