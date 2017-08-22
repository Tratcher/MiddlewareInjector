using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MiddlewareInjector
{
    public class MiddlewareInjectorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationBuilder _builder;
        private readonly MiddlewareInjectorOptions _options;
        private RequestDelegate _subPipeline;

        public MiddlewareInjectorMiddleware(RequestDelegate next, IApplicationBuilder builder, MiddlewareInjectorOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task Invoke(HttpContext httpContext)
        {
            var injector = _options.GetInjector();
            if (injector != null)
            {
                var builder = _builder.New();
                injector(builder);
                builder.Run(_next);
                _subPipeline = builder.Build();
            }

            if (_subPipeline != null)
            {
                return _subPipeline(httpContext);
            }

            return _next(httpContext);
        }
    }
}
