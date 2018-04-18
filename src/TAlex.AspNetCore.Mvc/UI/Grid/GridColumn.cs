using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using TAlex.AspNetCore.Mvc.Sorting;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    public class GridColumn<T> : IGridColumn<T> where T : class
    {
        private string displayName;
        private bool doNotSplit;
        private readonly Func<T, object> columnValueFunc;
        private Func<T, bool> cellCondition = x => true;
        private string format;
        private bool htmlEncode = true;
        private List<Func<GridRowViewData<T>, IDictionary<string, object>>> attributes = new List<Func<GridRowViewData<T>, IDictionary<string, object>>>();
        private Func<object, object> headerRenderer = x => null;

        /// <summary>
        /// Creates a new instance of the GridColumn class
        /// </summary>
        public GridColumn(Func<T, object> columnValueFunc, string name, Type type)
        {
            Name = name;
            this.displayName = name;
            ColumnType = type;
            this.columnValueFunc = columnValueFunc;
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
                if (this.doNotSplit)
                {
                    return this.displayName;
                }
                return SplitPascalCase(this.displayName);
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
            this.attributes.Add(attributes);
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
            var pairs = this.attributes.SelectMany(attributeFunc => attributeFunc(row));

            foreach (var pair in pairs)
            {
                dictionary[pair.Key] = pair.Value;
            }

            return dictionary;
        }

        public IGridColumn<T> Named(string name)
        {
            this.displayName = name;
            this.doNotSplit = true;
            return this;
        }

        public IGridColumn<T> DoNotSplit()
        {
            this.doNotSplit = true;
            return this;
        }

        public IGridColumn<T> Format(string format)
        {
            this.format = format;
            return this;
        }

        public IGridColumn<T> CellCondition(Func<T, bool> func)
        {
            this.cellCondition = func;
            return this;
        }

        IGridColumn<T> IGridColumn<T>.Visible(bool isVisible)
        {
            Visible = isVisible;
            return this;
        }

        public IGridColumn<T> Header(Func<object, object> headerRenderer)
        {
            this.headerRenderer = headerRenderer;
            return this;
        }

        public IGridColumn<T> Encode(bool shouldEncode)
        {
            this.htmlEncode = shouldEncode;
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
        public string GetValue(T instance)
        {
            if (!this.cellCondition(instance))
            {
                return null;
            }

            var value = this.columnValueFunc(instance);

            if (!string.IsNullOrEmpty(this.format))
            {
                value = string.Format(this.format, HtmlContentToString(value));
            }

            if (this.htmlEncode && value != null && !(value is IHtmlContent))
            {
                value = HttpUtility.HtmlEncode(value.ToString());
            }

            return HtmlContentToString(value);
        }

        public string GetHeader()
        {
            var header = this.headerRenderer(null);
            return HtmlContentToString(header);
        }

        private static string HtmlContentToString(object obj)
        {
            if (obj is IHtmlContent)
            {
                var html = obj as IHtmlContent;

                using (var writer = new StringWriter())
                {
                    html.WriteTo(writer, HtmlEncoder.Default);
                    return writer.ToString();
                }
            }

            return obj?.ToString();
        }
    }
}
