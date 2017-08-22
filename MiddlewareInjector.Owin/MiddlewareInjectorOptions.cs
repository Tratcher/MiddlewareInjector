using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Owin;

namespace MiddlewareInjector
{
    public class MiddlewareInjectorOptions
    {
        private Action<IAppBuilder> _injector;

        public void InjectMiddleware(Action<IAppBuilder> builder)
        {
            Interlocked.Exchange(ref _injector, builder);
        }

        internal Action<IAppBuilder> GetInjector()
        {
            return Interlocked.Exchange(ref _injector, null);
        }
    }
}
