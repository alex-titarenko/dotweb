using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TAlex.Web.Mvc.Pagination
{
    public class LazyPagination<TSource, TResult> : IPagination<TResult>
    {
        #region Fields

        /// <summary>
        /// Default page size.
        /// </summary>
        public const int DefaultPageSize = 20;

        private int _totalItems;

        private IList<TResult> _results;

        #endregion

        #region Properties

        public IQueryable<TSource> Query
        {
            get;
            protected set;
        }

        public Func<TSource, TResult> Converter { get; protected set; }

        #endregion

        #region Constructors

        public LazyPagination(IQueryable<TSource> query, int pageNumber, int pageSize, Func<TSource, TResult> converter)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			Query = query;
            Converter = converter;
		}

        #endregion

        #region IPagination Members

        public int PageNumber
        {
            get;
            private set;
        }

        public int PageSize
        {
            get;
            private set;
        }

        public int TotalItems
        {
            get
            {
                LazyLoadQuery();
                return _totalItems;
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(((double)TotalItems) / PageSize);
            }
        }

        public int FirstItem
        {
            get
            {
                LazyLoadQuery();
                return ((PageNumber - 1) * PageSize) + 1;
            }
        }

        public int LastItem
        {
            get
            {
                return FirstItem + _results.Count - 1;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageNumber > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageNumber < TotalPages;
            }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<TResult>)this).GetEnumerator();
        }

        #endregion

        #region IEnumerable<TResult> Members

        IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
        {
            LazyLoadQuery();

            foreach (var item in _results)
            {
                yield return item;
            }
        }

        #endregion

        #region Helpers

        protected void LazyLoadQuery()
        {
            if (_results == null)
            {
                _totalItems = Query.Count();

                int numberToSkip = (PageNumber - 1) * PageSize;

                var page = Query.Skip(numberToSkip).Take(PageSize).ToList();
                _results = page.Select(Converter).ToList();
            }
        }

        #endregion
    }
}
