using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;

namespace MiddlewareInjector
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class MiddlewareInjectorMiddleware
    {
        private readonly AppFunc _next;
        private readonly IAppBuilder _builder;
        private readonly MiddlewareInjectorOptions _options;
        private AppFunc _subPipeline;

        public MiddlewareInjectorMiddleware(AppFunc next, IAppBuilder builder, MiddlewareInjectorOptions options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            var injector = _options.GetInjector();
            if (injector != null)
            {
                var builder = _builder.New();
                injector(builder);
                builder.Use(new Func<AppFunc, AppFunc>(_ => _next));
                _subPipeline = builder.Build();
            }

            if (_subPipeline != null)
            {
                return _subPipeline(environment);
            }

            return _next(environment);
        }
    }
}
