using EPiBootstrapArea;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Framework;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Features.Header;
using Foundation.Features.Home;
using Foundation.Features.Locations.LocationItemPage;
using Foundation.Features.Locations.LocationListPage;
using Foundation.Features.Search;
using Foundation.Find.Cms;
using Foundation.Infrastructure.Display;
using Foundation.Infrastructure.PowerSlices;
using Foundation.Infrastructure.SchemaMarkup;
using PowerSlice;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Owin;
using System.Web.Mvc;

namespace Foundation.Infrastructure
{
    [ModuleDependency(typeof(Cms.Initialize))]
    [ModuleDependency(typeof(EPiServer.ServiceApi.IntegrationInitialization))]
    [ModuleDependency(typeof(EPiServer.ContentApi.Core.Internal.ContentApiCoreInitialization))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        private IServiceConfigurationProvider _services;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {

            context.ConfigureFoundationCms();
            context.Services.Configure<ContentApiConfiguration>(c =>
            {
                c.EnablePreviewFeatures = true;
                c.Default(RestVersion.Version_3_0)
                    .SetMinimumRoles(string.Empty)
                    .SetRequiredRole(string.Empty);
                c.Default(RestVersion.Version_2_0)
                    .SetMinimumRoles(string.Empty)
                    .SetRequiredRole(string.Empty);
            });

            context.Services.Configure<ContentApiSearchConfiguration>(config =>
            {
                config.Default()
                .SetMaximumSearchResults(200)
                .SetSearchCacheDuration(TimeSpan.FromMinutes(60));
            });

            context.Services.AddSingleton<IDisplayModeFallbackProvider, FoundationDisplayModeProvider>();
            context.Services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            context.Services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            context.Services.AddSingleton<IHeaderViewModelFactory, HeaderViewModelFactory>();
            context.Services.AddSingleton<BlogTagFactory>();
            context.Services.AddSingleton<ISearchService, SearchService>();
            context.Services.AddSingleton<ISearchViewModelFactory, SearchViewModelFactory>();
            context.Services.AddSingleton<IModelBinderProvider, FindModelBinderProvider>();
            context.Services.AddTransient<IContentQuery, LandingPagesSlice>();
            context.Services.AddTransient<IContentSlice, LandingPagesSlice>();
            context.Services.AddTransient<IContentQuery, StandardPagesSlice>();
            context.Services.AddTransient<IContentSlice, StandardPagesSlice>();
            context.Services.AddTransient<IContentQuery, BlogsSlice>();
            context.Services.AddTransient<IContentSlice, BlogsSlice>();
            context.Services.AddTransient<IContentQuery, BlocksSlice>();
            context.Services.AddTransient<IContentSlice, BlocksSlice>();
            context.Services.AddTransient<IContentQuery, MediaSlice>();
            context.Services.AddTransient<IContentSlice, MediaSlice>();
            context.Services.AddTransient<IContentQuery, ImagesSlice>();
            context.Services.AddTransient<IContentSlice, ImagesSlice>();
            context.Services.AddTransient<IContentQuery, EverythingSlice>();
            context.Services.AddTransient<IContentSlice, EverythingSlice>();
            context.Services.AddTransient<IContentQuery, MyContentSlice>();
            context.Services.AddTransient<IContentSlice, MyContentSlice>();
            context.Services.AddTransient<IContentQuery, MyPagesSlice>();
            context.Services.AddTransient<IContentSlice, MyPagesSlice>();
            context.Services.AddTransient<IContentQuery, UnusedMediaSlice>();
            context.Services.AddTransient<IContentSlice, UnusedMediaSlice>();
            context.Services.AddTransient<IContentQuery, UnusedBlocksSlice>();
            context.Services.AddTransient<IContentSlice, UnusedBlocksSlice>();
            context.Services.AddSingleton<ISchemaDataMapper<BlogItemPage>, BlogItemPageSchemaMapper>();
            context.Services.AddSingleton<ISchemaDataMapper<HomePage>, HomePageSchemaMapper>();
            context.Services.AddSingleton<ISchemaDataMapper<LocationItemPage>, LocationItemPageSchemaDataMapper>();
        }

        public void Initialize(InitializationEngine context)
        {
            context.InitializeFoundationFindCms();

            var handler = GlobalConfiguration.Configuration.MessageHandlers
                .FirstOrDefault(x => x.GetType() == typeof(PassiveAuthenticationMessageHandler));

            ViewEngines.Engines.Insert(0, new FeaturesViewEngine());

            if (handler != null)
            {
                GlobalConfiguration.Configuration.MessageHandlers.Remove(handler);
            }

            context.InitComplete += ContextOnInitComplete;

            SearchClient.Instance.Conventions.UnifiedSearchRegistry.ForInstanceOf<LocationListPage>()
                .ProjectImageUriFrom(page => new Uri(context.Locate.Advanced.GetInstance<UrlResolver>().GetUrl(page.PageImage), UriKind.Relative));

            SearchClient.Instance.Conventions.ForInstancesOf<LocationItemPage>().IncludeField(dp => dp.TagString());
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.InitComplete -= ContextOnInitComplete;
        }

        private void ContextOnInitComplete(object sender, EventArgs eventArgs)
        {
            _services.AddTransient<ContentAreaRenderer, FoundationContentAreaRenderer>();
        }
    }
}