using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAlex.Web.CacheProviders
{
    public interface ICacheProvider
    {
        IEnumerable<string> GetAllKeys();

        object Get(string key);

        void Remove(string key);

        void Clear();
    }
}
