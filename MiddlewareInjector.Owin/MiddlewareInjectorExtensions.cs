using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Owin;

namespace MiddlewareInjector
{
    public static class MiddlewareInjectorExtensions
    {
        public static IAppBuilder UseMiddlewareInjector(this IAppBuilder builder, MiddlewareInjectorOptions options)
        {
            return builder.Use<MiddlewareInjectorMiddleware>(builder.New(), options);
        }
    }
}
