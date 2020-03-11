using Couchbase;
using Couchbase.Configuration.Client;
using Couchbase.Configuration.Server.Providers;
using Couchbase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Cache
{
    public class CouchBaseCacheAdapter : IWebCache
    {
        static ClientConfiguration config = null;
        private readonly Cluster cluster = null;
        IBucket _ibucket = null;
        public CouchBaseCacheAdapter()
        {
            config = new ClientConfiguration
            {
                Servers = new List<Uri>
                    {
                    //new Uri("http://127.0.0.1:8091/pools"),
                    new Uri("http://127.0.0.1:8091")
                    }
            };
            //config.ConfigurationProviders = ServerConfigurationProviders.HttpStreaming;
            cluster = new Cluster(config);
            cluster.Authenticate("Administrator", "cegth20");//from your couchbase
            _ibucket = cluster.OpenBucket("mybucket");
        }
        // for CouchBase
        #region IWebCache Members
        public void Remove(string key)
        {
            try
            {
                var removed = _ibucket.Remove(key);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void Store(string key, object obj)
        {
            try
            {
                var upsert = _ibucket.Upsert(key, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public T Retrieve<T>(string key)
        {
            T cachedData = default(T);
            var data = _ibucket.GetDocument<T>(key);
            cachedData = data.Document.Content;
            return cachedData;
        }
        #endregion
    }
}


