namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    public interface IGrid<T> : IGridWithOptions<T> where T : class
    {
        /// <summary>
        /// Specifies a custom GridModel to use.
        /// </summary>
        /// <param name="model">The GridModel storing information about this grid</param>
        /// <returns></returns>
        IGrid<T> WithModel(IGridModel<T> model);
    }
}
