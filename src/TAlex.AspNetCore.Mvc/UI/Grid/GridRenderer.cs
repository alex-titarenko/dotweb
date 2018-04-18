using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    /// <summary>
	/// Base class for Grid Renderers. 
	/// </summary>
	public abstract class GridRenderer<T> : IGridRenderer<T> where T : class
    {
        protected IGridModel<T> GridModel { get; private set; }

        protected IEnumerable<T> DataSource { get; private set; }

        protected ViewContext Context { get; private set; }

        protected TextWriter Writer { get; private set; }

        protected GridRenderer()
        {
        }

        public void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output, ViewContext context)
        {
            Writer = output;
            GridModel = gridModel;
            DataSource = dataSource;
            Context = context;

            RenderGridStart();
            bool hasItems = RenderHeader();

            if (hasItems)
            {
                RenderItems();
            }
            else
            {
                RenderEmpty();
            }

            RenderGridEnd(!hasItems);
        }

        protected void RenderText(string text)
        {
            Writer.Write(text);
        }

        protected virtual void RenderItems()
        {
            RenderBodyStart();

            bool isAlternate = false;
            foreach (var item in DataSource)
            {
                RenderItem(new GridRowViewData<T>(item, isAlternate));
                isAlternate = !isAlternate;
            }

            RenderBodyEnd();
        }

        protected virtual void RenderItem(GridRowViewData<T> rowData)
        {
            BaseRenderRowStart(rowData);

            foreach (var column in VisibleColumns())
            {
                RenderStartCell(column, rowData);
                RenderCellValue(column, rowData);
                RenderEndCell();
            }

            BaseRenderRowEnd(rowData);
        }

        protected virtual void RenderCellValue(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            var cellValue = column.GetValue(rowData.Item);

            if (!string.IsNullOrEmpty(cellValue))
            {
                RenderText(cellValue);
            }
        }

        protected virtual bool RenderHeader()
        {
            //No items - do not render a header.
            if (!ShouldRenderHeader()) return false;

            RenderHeadStart();

            foreach (var column in VisibleColumns())
            {
                RenderHeaderCellStart(column);
                RenderHeaderText(column);
                RenderHeaderCellEnd();
            }

            RenderHeadEnd();

            return true;
        }

        protected virtual void RenderHeaderText(GridColumn<T> column)
        {
            var customHeader = column.GetHeader();

            if (customHeader != null)
            {
                RenderText(customHeader);
            }
            else
            {
                RenderText(column.DisplayName);
            }
        }

        protected virtual bool ShouldRenderHeader()
        {
            return !IsDataSourceEmpty();
        }

        protected bool IsDataSourceEmpty()
        {
            return DataSource == null || !DataSource.Any();
        }

        protected IEnumerable<GridColumn<T>> VisibleColumns()
        {
            return GridModel.Columns.Where(x => x.Visible);
        }

        protected void BaseRenderRowStart(GridRowViewData<T> rowData)
        {
            bool rendered = GridModel.Sections.Row.StartSectionRenderer(rowData, new RenderingContext(Writer, Context));

            if (!rendered)
            {
                RenderRowStart(rowData);
            }
        }

        protected void BaseRenderRowEnd(GridRowViewData<T> rowData)
        {
            bool rendered = GridModel.Sections.Row.EndSectionRenderer(rowData, new RenderingContext(Writer, Context));

            if (!rendered)
            {
                RenderRowEnd();
            }
        }

        protected bool IsSortingEnabled
        {
            get { return GridModel.SortOptions != null; }
        }

        protected abstract void RenderHeaderCellEnd();

        protected abstract void RenderHeaderCellStart(GridColumn<T> column);

        protected abstract void RenderRowStart(GridRowViewData<T> rowData);

        protected abstract void RenderRowEnd();

        protected abstract void RenderEndCell();

        protected abstract void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowViewData);

        protected abstract void RenderHeadStart();

        protected abstract void RenderHeadEnd();

        protected abstract void RenderGridStart();

        protected abstract void RenderGridEnd(bool isEmpty);

        protected abstract void RenderEmpty();

        protected abstract void RenderBodyStart();

        protected abstract void RenderBodyEnd();
    }
}
