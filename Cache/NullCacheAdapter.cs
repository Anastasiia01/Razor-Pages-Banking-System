using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Cache
{
    public class NullObjectCache : IWebCache
    { // so that no caching can be easily implemented
        public void Remove(string key)
        {
        }
        public void Store(string key, object obj)
        {
        }
        public T Retrieve<T>(string key)
        {
            return default(T);
        }
    }
}
