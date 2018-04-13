using System;
using System.Collections;
using System.Collections.Generic;


namespace TAlex.AspNetCore.Mvc.Pagination
{
    /// <summary>
    /// Represents a collection of objects that has been split into pages.
    /// </summary>
    public interface IPagination : IEnumerable
    {
        /// <summary>
        /// Gets the current page number.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets the number of items in each page.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets the total number of items.
        /// </summary>
        int TotalItems { get; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Gets the index of the first item in the page.
        /// </summary>
        int FirstItem { get; }

        /// <summary>
        /// Gets the index of the last item in the page.
        /// </summary>
        int LastItem { get; }

        /// <summary>
        /// Gets a value that indicates whether there are pages before the current page.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Gets a value that indicates whether there are pages after the current page.
        /// </summary>
        bool HasNextPage { get; }
    }


    /// <summary>
    /// Represents a generic collection of objects that has been split into pages.
    /// </summary>
    /// <typeparam name="T">Type of object being paged.</typeparam>
    public interface IPagination<T> : IPagination, IEnumerable<T>
    {
    }
}
