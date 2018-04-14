using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TAlex.AspNetCore.Mvc.Sorting;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    public class GridColumn<T> : IGridColumn<T> where T : class
    {
        private string _displayName;
        private bool _doNotSplit;
        private readonly Func<T, object> _columnValueFunc;
        private Func<T, bool> _cellCondition = x => true;
        private string _format;
        private bool _htmlEncode = true;
        private List<Func<GridRowViewData<T>, IDictionary<string, object>>> _attributes = new List<Func<GridRowViewData<T>, IDictionary<string, object>>>();
        private Func<object, object> _headerRenderer = x => null;

        /// <summary>
        /// Creates a new instance of the GridColumn class
        /// </summary>
        public GridColumn(Func<T, object> columnValueFunc, string name, Type type)
        {
            Name = name;
            _displayName = name;
            ColumnType = type;
            _columnValueFunc = columnValueFunc;
        }

        public bool Sortable { get; private set; } = true;

        public bool Visible { get; private set; } = true;

        public string SortColumnName { get; private set; } = null;

        public SortDirection? InitialDirection { get; private set; }

        /// <summary>
        /// Name of the column
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Display name for the column
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (_doNotSplit)
                {
                    return _displayName;
                }
                return SplitPascalCase(_displayName);
            }
        }

        /// <summary>
        /// The type of the object being rendered for thsi column. 
        /// Note: this will return null if the type cannot be inferred.
        /// </summary>
        public Type ColumnType { get; }

        public int? Position { get; private set; }

        IGridColumn<T> IGridColumn<T>.Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
        {
            _attributes.Add(attributes);
            return this;
        }

        IGridColumn<T> IGridColumn<T>.Sortable(bool isColumnSortable)
        {
            Sortable = isColumnSortable;
            return this;
        }

        IGridColumn<T> IGridColumn<T>.SortColumnName(string name)
        {
            SortColumnName = name;
            return this;
        }

        IGridColumn<T> IGridColumn<T>.SortInitialDirection(SortDirection initialDirection)
        {
            InitialDirection = initialDirection;
            return this;
        }

        IGridColumn<T> IGridColumn<T>.InsertAt(int index)
        {
            Position = index;
            return this;
        }

        /// <summary>
        /// Additional attributes for the column header
        /// </summary>
        public IDictionary<string, object> HeaderAttributes { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Additional attributes for the cell
        /// </summary>
        public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes
        {
            get { return GetAttributesFromRow; }
        }

        private IDictionary<string, object> GetAttributesFromRow(GridRowViewData<T> row)
        {
            var dictionary = new Dictionary<string, object>();
            var pairs = _attributes.SelectMany(attributeFunc => attributeFunc(row));

            foreach (var pair in pairs)
            {
                dictionary[pair.Key] = pair.Value;
            }

            return dictionary;
        }

        public IGridColumn<T> Named(string name)
        {
            _displayName = name;
            _doNotSplit = true;
            return this;
        }

        public IGridColumn<T> DoNotSplit()
        {
            _doNotSplit = true;
            return this;
        }

        public IGridColumn<T> Format(string format)
        {
            _format = format;
            return this;
        }

        public IGridColumn<T> CellCondition(Func<T, bool> func)
        {
            _cellCondition = func;
            return this;
        }

        IGridColumn<T> IGridColumn<T>.Visible(bool isVisible)
        {
            Visible = isVisible;
            return this;
        }

        public IGridColumn<T> Header(Func<object, object> headerRenderer)
        {
            _headerRenderer = headerRenderer;
            return this;
        }

        public IGridColumn<T> Encode(bool shouldEncode)
        {
            _htmlEncode = shouldEncode;
            return this;
        }

        IGridColumn<T> IGridColumn<T>.HeaderAttributes(IDictionary<string, object> attributes)
        {
            foreach (var attribute in attributes)
            {
                HeaderAttributes.Add(attribute);
            }

            return this;
        }

        private string SplitPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        /// <summary>
        /// Gets the value for a particular cell in this column
        /// </summary>
        /// <param name="instance">Instance from which the value should be obtained</param>
        /// <returns>Item to be rendered</returns>
        public object GetValue(T instance)
        {
            if (!_cellCondition(instance))
            {
                return null;
            }

            var value = _columnValueFunc(instance);

            if (!string.IsNullOrEmpty(_format))
            {
                value = string.Format(_format, value);
            }

            if (_htmlEncode && value != null && !(value is IHtmlContent))
            {
                value = HttpUtility.HtmlEncode(value.ToString());
            }

            return value;
        }

        public string GetHeader()
        {
            var header = _headerRenderer(null);
            return header == null ? null : header.ToString();
        }
    }
}
