using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;

namespace TAlex.AspNetCore.Mvc.UI.Grid
{
    /// <summary>
    /// Context class used when rendering the grid.
    /// </summary>
    public class RenderingContext
    {
        public TextWriter Writer { get; }

        public ViewContext ViewContext { get; }

        public RenderingContext(TextWriter writer, ViewContext viewContext)
        {
            Writer = writer;
            ViewContext = viewContext;
        }
    }
}
