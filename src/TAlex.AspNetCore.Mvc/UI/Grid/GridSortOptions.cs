using TAlex.AspNetCore.Mvc.Sorting;


namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    /// <summary>
	/// Sorting information for use with the grid.
	/// </summary>
	public class GridSortOptions
    {
        public string Column { get; set; }

        public SortDirection Direction { get; set; }
    }
}
