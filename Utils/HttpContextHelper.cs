using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Utils
{
    public class HttpContextHelper
    {
        // purpose is to provide access to HttpContext in any page or class
        // HttpContext is an important object that contains info about the
        // current request, response, authentication info, Session etc..
        // In a page, HttpContext is made available automatically by the
        // Context object. However, in a code class, we need the HttpContextHelper
        static IHttpContextAccessor _ihttpCtxAccessor = null;
        public static void Configure(IHttpContextAccessor accessor)
        {
            _ihttpCtxAccessor = accessor;
        }
        public static HttpContext HttpCtx
        {
            get { return _ihttpCtxAccessor.HttpContext; }
        }
    }
}
