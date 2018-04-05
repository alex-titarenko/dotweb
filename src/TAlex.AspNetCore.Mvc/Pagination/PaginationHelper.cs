using System;
using System.Collections.Generic;
using System.Linq;


namespace TAlex.Web.Mvc.Pagination
{
    /// <summary>
	/// Extension methods for creating paged lists.
	/// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Converts the specified IEnumerable into an IPagination using the default page size and returns the specified page number.
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="source">Source enumerable to convert to the paged list.</param>
        /// <param name="pageNumber">The page number to return.</param>
        /// <returns>An IPagination of T</returns>
        public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber)
        {
            return source.AsPagination(pageNumber, LazyPagination<T, T>.DefaultPageSize);
        }

        /// <summary>
        /// Converts the specified IQueryable into an IPagination using the default page size and returns the specified page number.
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="source">Source queryable to convert to the paged list.</param>
        /// <param name="pageNumber">The page number to return.</param>
        /// <returns>An IPagination of T</returns>
        public static IPagination<T> AsPagination<T>(this IQueryable<T> source, int pageNumber)
        {
            return source.AsPagination(pageNumber, LazyPagination<T, T>.DefaultPageSize);
        }

        /// <summary>
        /// Converts the speciied IEnumerable into an IPagination using the specified page size and returns the specified page. 
        /// </summary>
        /// <typeparam name="T">Type of object in the collection</typeparam>
        /// <param name="source">Source enumerable to convert to the paged list.</param>
        /// <param name="pageNumber">The page number to return.</param>
        /// <param name="pageSize">Number of objects per page.</param>
        /// <returns>An IPagination of T</returns>
        public static IPagination<T> AsPagination<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
            }

            return new LazyPagination<T, T>(source.AsQueryable(), pageNumber, pageSize, x => x);
        }

        /// <summary>
        /// Converts the speciied IQueryable into an IPagination using the specified page size and returns the specified page. 
        /// </summary>
        /// <typeparam name="T">Type of object in the collection.</typeparam>
        /// <param name="source">Source queryable to convert to the paged list.</param>
        /// <param name="pageNumber">The page number to return.</param>
        /// <param name="pageSize">Number of objects per page.</param>
        /// <returns>An IPagination of T</returns>
        public static IPagination<T> AsPagination<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
            }

            return new LazyPagination<T, T>(source, pageNumber, pageSize, x => x);
        }

        /// <summary>
        /// Converts the speciied IQueryable into an IPagination using the specified page size and entity converter and returns the specified page.
        /// </summary>
        /// <typeparam name="TSource">Type of source object in the collection.</typeparam>
        /// <typeparam name="TResult">Type of result object in the collection.</typeparam>
        /// <param name="source">Source queryable to convert to the paged list.</param>
        /// <param name="pageNumber">The page number to return.</param>
        /// <param name="pageSize">Number of objects per page.</param>
        /// <param name="converter">A function that specifies mapping between source type and result.</param>
        /// <returns>An IPagination of TResult</returns>
        public static IPagination<TResult> AsPagination<TSource, TResult>(this IQueryable<TSource> source, int pageNumber, int pageSize, Func<TSource, TResult> converter)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("pageNumber", "The page number should be greater than or equal to 1.");
            }

            return new LazyPagination<TSource, TResult>(source, pageNumber, pageSize, converter);
        }
    }
}
