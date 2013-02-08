using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAlex.Web.CacheProviders
{
    public class CacheProvidersManager : ICacheProvidersManager
    {
        private readonly IList<ICacheProvider> _cacheProviders;

        public CacheProvidersManager(IEnumerable<ICacheProvider> cacheProviders)
        {
            _cacheProviders = cacheProviders.ToList();
        }

        public object Get(string key)
        {
            foreach (ICacheProvider cacheProvider in _cacheProviders)
            {
                object value = cacheProvider.Get(key);
                if (value != null) return value;
            }

            return null;
        }

        public void Remove(string key)
        {
            _cacheProviders.AsParallel().ForAll(p => p.Remove(key));
        }

        public void Clear()
        {
            _cacheProviders.AsParallel().ForAll(p => p.Clear());
        }

        public IEnumerable<string> GetAllKeys()
        {
            return _cacheProviders.AsParallel().SelectMany(x => x.GetAllKeys());
        }
    }
}
