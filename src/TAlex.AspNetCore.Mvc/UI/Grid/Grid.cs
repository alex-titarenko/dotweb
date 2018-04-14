using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;


namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    /// <summary>
	/// Defines a grid to be rendered.
	/// </summary>
	/// <typeparam name="T">Type of datasource for the grid</typeparam>
	public class Grid<T> : IGrid<T> where T : class
    {
        private readonly ViewContext _context;

        /// <summary>
        /// The GridModel that holds the internal representation of this grid.
        /// </summary>
        public IGridModel<T> Model { get; private set; }

        /// <summary>
        /// Creates a new instance of the Grid class.
        /// </summary>
        /// <param name="dataSource">The datasource for the grid</param>
        /// <param name="context"></param>
        public Grid(IEnumerable<T> dataSource, ViewContext context)
        {
            this._context = context;
            DataSource = dataSource;

            this.Model = new GridModel<T>(this.GetMetadataProvider());
        }

        /// <summary>
        /// The datasource for the grid.
        /// </summary>
        public IEnumerable<T> DataSource { get; private set; }

        public IGridWithOptions<T> RenderUsing(IGridRenderer<T> renderer)
        {
            Model.Renderer = renderer;
            return this;
        }

        public IGridWithOptions<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
        {
            var builder = new ColumnBuilder<T>(this.GetMetadataProvider());
            columnBuilder(builder);

            foreach (var column in builder)
            {
                if (column.Position == null)
                {
                    Model.Columns.Add(column);
                }
                else
                {
                    Model.Columns.Insert(column.Position.Value, column);
                }
            }

            return this;
        }

        public IGridWithOptions<T> Empty(string emptyText)
        {
            Model.EmptyText = emptyText;
            return this;
        }

        public IGridWithOptions<T> Attributes(IDictionary<string, object> attributes)
        {
            Model.Attributes = attributes;
            return this;
        }

        public IGrid<T> WithModel(IGridModel<T> model)
        {
            Model = model;
            return this;
        }

        public IGridWithOptions<T> Sort(GridSortOptions sortOptions)
        {
            Model.SortOptions = sortOptions;
            return this;
        }

        public IGridWithOptions<T> Sort(GridSortOptions sortOptions, string prefix)
        {
            Model.SortOptions = sortOptions;
            Model.SortPrefix = prefix;
            return this;
        }

        public override string ToString()
        {
            return ToHtmlString();
        }

        public string ToHtmlString()
        {
            var writer = new StringWriter();
            Model.Renderer.Render(Model, DataSource, writer, _context);
            return writer.ToString();
        }

        public IGridWithOptions<T> HeaderRowAttributes(IDictionary<string, object> attributes)
        {
            Model.Sections.HeaderRowAttributes(attributes);
            return this;
        }

        public IGridWithOptions<T> RowAttributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes)
        {
            Model.Sections.RowAttributes(attributes);
            return this;
        }

        public void Render()
        {
            throw new NotImplementedException();
        }

        public void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            Model.Renderer.Render(Model, DataSource, writer, _context);
        }

        private IModelMetadataProvider GetMetadataProvider()
        {
            var services = this._context.HttpContext.RequestServices;
            var metadataProvider = (IModelMetadataProvider)services.GetService(typeof(IModelMetadataProvider));

            return metadataProvider;
        }
    }
}
