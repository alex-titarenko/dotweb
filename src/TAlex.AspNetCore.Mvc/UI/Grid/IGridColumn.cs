using System;
using System.Collections.Generic;
using System.Text;
using TAlex.AspNetCore.Mvc.Sorting;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    public interface IGridColumn<T>
    {
        IGridColumn<T> Named(string name);

        IGridColumn<T> DoNotSplit();

        IGridColumn<T> Format(string format);

        IGridColumn<T> CellCondition(Func<T, bool> func);

        IGridColumn<T> Visible(bool isVisible);

        IGridColumn<T> Header(Func<object, object> customHeaderRenderer);

        IGridColumn<T> Encode(bool shouldEncode);

        IGridColumn<T> HeaderAttributes(IDictionary<string, object> attributes);

        IGridColumn<T> Attributes(Func<GridRowViewData<T>, IDictionary<string, object>> attributes);

        IGridColumn<T> Sortable(bool isColumnSortable);

        IGridColumn<T> SortColumnName(string name);

        IGridColumn<T> SortInitialDirection(SortDirection initialDirection);

        IGridColumn<T> InsertAt(int index);
    }
}
