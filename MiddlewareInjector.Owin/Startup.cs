using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MiddlewareInjector.Owin.Startup))]

namespace MiddlewareInjector.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var injectorOptions = new MiddlewareInjectorOptions();

            app.UseMiddlewareInjector(injectorOptions);

            app.Run(async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("<html><body>");
                if (context.Request.Path.Equals(new PathString("/clear")))
                {
                    injectorOptions.InjectMiddleware(_ => { });
                    await context.Response.WriteAsync("Cleared middleware<br>");
                }
                else if (context.Request.Path.Equals(new PathString("/inject")))
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

        public void InjectContent(IAppBuilder builder)
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
