using System;
using System.Collections.Generic;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    /// <summary>
    /// Represents a Grid Row
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridRow<T>
    {
        /// <summary>
        /// Invokes the custom renderer defined (if any) for the start of the row. 
        /// Returns TRUE if custom rendering occurred (indicating that further rendering should stop) otherwise FALSE.
        /// </summary>
        public Func<GridRowViewData<T>, RenderingContext, bool> StartSectionRenderer { get; set; } = (x, y) => false;

        /// <summary>
        /// Invokes the custom renderer defined (if any) for the start of the row.
        /// Returns TRUE if custom rendering occurred (indicating that further rendering should stop) otherwise FALSE.
        /// </summary>
        public Func<GridRowViewData<T>, RenderingContext, bool> EndSectionRenderer { get; set; } = (x, y) => false;

        /// <summary>
        /// Returns custom attributes for the row.
        /// </summary>
        public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes { get; set; } = x => new Dictionary<string, object>();
    }
}
