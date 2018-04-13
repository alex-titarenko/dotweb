using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Dynamic;

namespace TAlex.AspNetCore.Mvc.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        public static object ToRouteValues(this RouteValueDictionary source)
        {
            var eo = new ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;
            
            foreach (var kvp in source)
            {
                eoColl.Add(kvp);
            }

            return eo;
        }
    }
}
