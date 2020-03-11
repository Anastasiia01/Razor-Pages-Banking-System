using BankRPEF.Utils;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Cache
{
    public class RedisCacheAdapter : IWebCache
    {
        public void Remove(string key)
        {
            RedisCacheHelper.IDISTCACHE.Remove(key);
        }
        public T Retrieve<T>(string key)
        {
            string data = RedisCacheHelper.IDISTCACHE.GetString(key);
            if (data != null)
                return JsonConvert.DeserializeObject<T>(data);
            else
                return default(T);
        }
        public void Store(string key, object obj)
        {
            string objdata = JsonConvert.SerializeObject(obj);
            RedisCacheHelper.IDISTCACHE.SetString(key, objdata);
        }
    }
}
