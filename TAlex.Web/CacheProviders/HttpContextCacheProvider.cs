using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TAlex.Web.CacheProviders
{
    public class HttpContextCacheProvider : ICacheProvider
    {
        private HttpContextBase _context;


        public HttpContextCacheProvider()
        {
            _context = new HttpContextWrapper(HttpContext.Current);
        }


        public IEnumerable<string> GetAllKeys()
        {
            return _context.Cache
                .Cast<System.Collections.DictionaryEntry>()
                .Select(x => (string)x.Key);
        }

        public object Get(string key)
        {
            return _context.Cache.Get(key);
        }

        public void Remove(string key)
        {
            _context.Cache.Remove(key);
        }

        public void Clear()
        {
            List<string> keys = GetAllKeys().ToList();
            keys.ForEach(x => Remove(x));
        }
    }
}
