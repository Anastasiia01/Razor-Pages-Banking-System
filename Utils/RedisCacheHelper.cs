using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Utils
{
    public class RedisCacheHelper
    {
        public static IDistributedCache IDISTCACHE = null;
    }
}
