namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    public interface IGridSections<T> where T : class
    {
        GridRow<T> Row { get; }
        GridRow<T> HeaderRow { get; }
    }
}
