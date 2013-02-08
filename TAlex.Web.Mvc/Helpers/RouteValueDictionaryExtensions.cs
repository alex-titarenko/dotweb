using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Routing;


namespace TAlex.Web.Mvc.Helpers
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary ToRouteValues(this NameValueCollection queryString)
        {
            if (queryString == null || queryString.HasKeys() == false) return new RouteValueDictionary();

            RouteValueDictionary routeValues = new RouteValueDictionary();
            foreach (string key in queryString.Keys)
                routeValues.Add(key, queryString[key]);

            return routeValues;
        }
    }
}
