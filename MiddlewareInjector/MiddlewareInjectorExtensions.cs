using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace MiddlewareInjector
{
    public static class MiddlewareInjectorExtensions
    {
        public static IApplicationBuilder UseMiddlewareInjector(this IApplicationBuilder builder, MiddlewareInjectorOptions options)
        {
            return builder.UseMiddleware<MiddlewareInjectorMiddleware>(builder.New(), options);
        }
    }
}
