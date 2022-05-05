using EPiServer.Authorization;
using EPiServer.Cms.TinyMce;
using EPiServer.ContentApi.Cms;
using EPiServer.ContentApi.Cms.Internal;
using EPiServer.ContentDefinitionsApi;
using EPiServer.ContentManagementApi;
using EPiServer.Find;
using EPiServer.Framework.Web.Resources;
using EPiServer.OpenIDConnect;
using EPiServer.ServiceLocation;
using EPiServer.Shell.Modules;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Infrastructure;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Display;
using Geta.NotFoundHandler.Infrastructure.Configuration;
using Geta.NotFoundHandler.Optimizely;
using Jhoose.Security.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

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
            services.AddCmsAspNetIdentity<SiteUser>();
            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(ro => ro.ViewLocationExpanders.Add(new FeatureViewLocationExpander()));

            if (_webHostingEnvironment.IsDevelopment())
            {
                services.Configure<ClientResourceOptions>(uiOptions => uiOptions.Debug = true);
            }

            services.AddCms();
            services.AddDisplay();
            services.AddTinyMce();
            services.AddFind();
            
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/util/Login";
                options.ExpireTimeSpan = new TimeSpan(0, 20, 0);
                options.SlidingExpiration = true;
            });
            services.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Singleton(typeof(IFirstRequestInitializer), typeof(ContentInstaller)));
            services.AddDetection();
            services.ConfigureContentApiOptions(o =>
            {
                o.EnablePreviewFeatures = true;
                o.IncludeEmptyContentProperties = true;
                o.FlattenPropertyModel = false;
                o.IncludeMasterLanguage = false;

            });

            // Content Delivery API
            services.AddContentDeliveryApi()
                .WithFriendlyUrl()
                .WithSiteBasedCors();

            // Content Definitions API
            services.AddContentDefinitionsApi(options =>
            {
                // Accept anonymous calls
                options.DisableScopeValidation = true;
            });

            // Content Management
            services.AddContentManagementApi(c =>
            {
                // Accept anonymous calls
                c.DisableScopeValidation = true;
            });
            services.AddOpenIDConnect<SiteUser>(options =>
            {
                //options.RequireHttps = !_webHostingEnvironment.IsDevelopment();
                var application = new OpenIDConnectApplication()
                {
                    ClientId = "postman-client",
                    ClientSecret = "postman",
                    Scopes =
                    {
                        ContentDeliveryApiOptionsDefaults.Scope,
                        ContentManagementApiOptionsDefaults.Scope,
                        ContentDefinitionsApiOptionsDefaults.Scope,
                    }
                };

                // Using Postman for testing purpose.
                // The authorization code is sent to postman after successful authentication.
                application.RedirectUris.Add(new Uri("https://oauth.pstmn.io/v1/callback"));
                options.Applications.Add(application);
                options.AllowResourceOwnerPasswordFlow = true;
            });

            services.AddOpenIDConnectUI();

            services.ConfigureContentDeliveryApiSerializer(settings => settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore);

            services.AddNotFoundHandler(o => o.UseSqlServer(_configuration.GetConnectionString("EPiServerDB")), policy => policy.RequireRole(Roles.CmsAdmins));
            services.AddOptimizelyNotFoundHandler();
            services.AddJhooseSecurity(_configuration);
            //services.AddContentManager();
            //services.AddGridView();

            services.Configure<ProtectedModuleOptions>(x =>
            {
                if (!x.Items.Any(x => x.Name.Equals("Foundation")))
                {
                    x.Items.Add(new ModuleDetails
                    {
                        Name = "Foundation"
                    });
                }
            });
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