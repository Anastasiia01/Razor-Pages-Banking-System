using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Cache
{
    public interface IWebCache
    {
        // A cache provider needs to implement this interface
        // via an adapter
        void Remove(string key);
        void Store(string key, object obj);
        T Retrieve<T>(string key);
    }
}
