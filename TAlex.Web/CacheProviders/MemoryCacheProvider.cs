using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace TAlex.Web.CacheProviders
{
    public class MemoryCacheProvider : ICacheProvider
    {
        public IEnumerable<string> GetAllKeys()
        {
            return MemoryCache.Default.Cast<KeyValuePair<string, object>>().Select(x => x.Key);
        }

        public object Get(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        public void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }

        public void Clear()
        {
            List<string> keys = GetAllKeys().ToList();
            keys.ForEach(x => Remove(x));
        }
    }
}
