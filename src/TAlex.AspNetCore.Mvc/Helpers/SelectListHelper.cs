using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace TAlex.Web.Mvc.Helpers
{
    public static class SelectListHelper
    {
        public static IEnumerable<SelectListItem> ListForBoolean(bool? value, string labelForTrue, string labelForFalse)
        {
            return new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Any", Value = "Any", Selected = !value.HasValue },
                new SelectListItem() { Text = labelForTrue, Value = "True", Selected = value.HasValue && value.Value },
                new SelectListItem() { Text = labelForFalse, Value = "False", Selected = value.HasValue && !value.Value }
            };
        }
    }
}
