using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Display;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;

namespace Foundation
{
    public class Startup
    {
        private readonly IWebHostEnvironment _webHostingEnvironment;
        private readonly IConfiguration _configuration;

        public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
        {
            _webHostingEnvironment = webHostingEnvironment;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCmsAspNetIdentity<SiteUser>(o =>
            {
                if (string.IsNullOrEmpty(o.ConnectionStringOptions?.ConnectionString))
                {
                    o.ConnectionStringOptions = new ConnectionStringOptions()
                    {
                        ConnectionString = _configuration.GetConnectionString("EPiServerDB")
                    };
                }
            });

            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(ro => ro.ConfigureFeatureFolders());

            services.AddCms();
            services.AddDisplay();
            services.AddTinyMce();
            services.AddFindUI(_configuration);
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/util/Login";
                options.ExpireTimeSpan = new TimeSpan(0, 20, 0);
                options.SlidingExpiration = true;
            });
            services.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Singleton(typeof(IFirstRequestInitializer), typeof(ContentInstaller)));
            services.AddDetection();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapContent();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

           
        }
    }
}