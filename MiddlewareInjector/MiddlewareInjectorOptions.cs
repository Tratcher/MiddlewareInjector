using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace MiddlewareInjector
{
    public class MiddlewareInjectorOptions
    {
        private Action<IApplicationBuilder> _injector;

        public void InjectMiddleware(Action<IApplicationBuilder> builder)
        {
            Interlocked.Exchange(ref _injector, builder);
        }

        internal Action<IApplicationBuilder> GetInjector()
        {
            return Interlocked.Exchange(ref _injector, null);
        }
    }
}
