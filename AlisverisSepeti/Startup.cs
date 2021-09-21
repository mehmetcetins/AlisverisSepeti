using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlisverisSepeti
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorRuntimeCompilation();

            
            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            var cookieProvider = options.Value.RequestCultureProviders
                .OfType<CookieRequestCultureProvider>()
                .First();
            var urlProvider = options.Value.RequestCultureProviders
                .OfType<QueryStringRequestCultureProvider>().First();
            cookieProvider.Options.DefaultRequestCulture = new RequestCulture(System.Globalization.CultureInfo.InvariantCulture);
            urlProvider.Options.DefaultRequestCulture = new RequestCulture(System.Globalization.CultureInfo.InvariantCulture);
            cookieProvider.CookieName = "UserCulture";
            options.Value.RequestCultureProviders.Clear();
            options.Value.RequestCultureProviders.Add(cookieProvider);
            options.Value.RequestCultureProviders.Add(urlProvider);
            app.UseRequestLocalization(options.Value);
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(System.IO.Path.Combine(env.ContentRootPath,"Public")),
                RequestPath= "/Public"
            }) ;
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                /*
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                */
                endpoints.MapControllerRoute(
                        name: "index",
                        pattern: "{controller=Home}/"
                    );
            });
        }
    }
}
