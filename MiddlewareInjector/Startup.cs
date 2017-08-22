using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MiddlewareInjector
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var injectorOptions = new MiddlewareInjectorOptions();

            app.UseMiddlewareInjector(injectorOptions);

            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<html><body>");
                if (context.Request.Path.Equals("/clear"))
                {
                    injectorOptions.InjectMiddleware(_ => { });
                    await context.Response.WriteAsync("Cleared middleware<br>");
                }
                else if (context.Request.Path.Equals("/inject"))
                {
                    injectorOptions.InjectMiddleware(InjectContent);

                    await context.Response.WriteAsync("Injected middleware<br>");
                }
                else
                {
                    await context.Response.WriteAsync("Hello World!<br>");
                }
                await context.Response.WriteAsync("<a href=\"/inject\">Inject</a><br>");
                await context.Response.WriteAsync("<a href=\"/testpath\">Test Path</a><br>");
                await context.Response.WriteAsync("<a href=\"/clear\">Clear</a><br>");
                await context.Response.WriteAsync("<a href=\"/\">Home</a><br>");
                await context.Response.WriteAsync("</body></html>");
            });
        }

        public void InjectContent(IApplicationBuilder builder)
        {
            builder.Map("/testpath", subBuilder =>
            {
                subBuilder.Run(async context =>
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("Injected content<br>");
                    await context.Response.WriteAsync("<a href=\"/\">Home</a><br>");
                    await context.Response.WriteAsync("</body></html>");
                });
            });
        }
    }
}
