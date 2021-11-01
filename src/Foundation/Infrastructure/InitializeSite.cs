using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Features.Header;
using Foundation.Features.Home;
using Foundation.Features.Locations.LocationItemPage;
using Foundation.Features.Search;
using Foundation.Features.Shared;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Extensions;
using Foundation.Infrastructure.Display;
using Foundation.Infrastructure.Find;
//using Foundation.Infrastructure.PowerSlices;
using Foundation.Infrastructure.SchemaMarkup;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
//using PowerSlice;
using System;

namespace Foundation.Infrastructure
{
    [ModuleDependency(typeof(Cms.Initialize))]
    //[ModuleDependency(typeof(EPiServer.ServiceApi.IntegrationInitialization))]
    //[ModuleDependency(typeof(EPiServer.ContentApi.Core.Internal.ContentApiCoreInitialization))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        private IServiceCollection _services;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
            context.ConfigureFoundationCms();

            //_services.Configure<ContentApiConfiguration>(c =>
            //{
            //    c.EnablePreviewFeatures = true;
            //    c.Default().SetMinimumRoles(string.Empty).SetRequiredRole(string.Empty);
            //});

            //_services.Configure<ContentApiSearchConfiguration>(config =>
            //{
            //    config.Default()
            //    .SetMaximumSearchResults(200)
            //    .SetSearchCacheDuration(TimeSpan.FromMinutes(60));
            //});

            //_services.AddSingleton<IDisplayModeFallbackProvider, FoundationDisplayModeProvider>();
            _services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            _services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            _services.AddSingleton<IHeaderViewModelFactory, HeaderViewModelFactory>();
            _services.AddSingleton<BlogTagFactory>();
            _services.AddSingleton<ISearchService, SearchService>();
            _services.AddSingleton<ISearchViewModelFactory, SearchViewModelFactory>();
            //_services.AddTransient<IContentQuery, LandingPagesSlice>();
            //_services.AddTransient<IContentSlice, LandingPagesSlice>();
            //_services.AddTransient<IContentQuery, StandardPagesSlice>();
            //_services.AddTransient<IContentSlice, StandardPagesSlice>();
            //_services.AddTransient<IContentQuery, BlogsSlice>();
            //_services.AddTransient<IContentSlice, BlogsSlice>();
            //_services.AddTransient<IContentQuery, BlocksSlice>();
            //_services.AddTransient<IContentSlice, BlocksSlice>();
            //_services.AddTransient<IContentQuery, MediaSlice>();
            //_services.AddTransient<IContentSlice, MediaSlice>();
            //_services.AddTransient<IContentQuery, ImagesSlice>();
            //_services.AddTransient<IContentSlice, ImagesSlice>();
            //_services.AddTransient<IContentQuery, EverythingSlice>();
            //_services.AddTransient<IContentSlice, EverythingSlice>();
            //_services.AddTransient<IContentQuery, MyContentSlice>();
            //_services.AddTransient<IContentSlice, MyContentSlice>();
            //_services.AddTransient<IContentQuery, MyPagesSlice>();
            //_services.AddTransient<IContentSlice, MyPagesSlice>();
            //_services.AddTransient<IContentQuery, UnusedMediaSlice>();
            //_services.AddTransient<IContentSlice, UnusedMediaSlice>();
            //_services.AddTransient<IContentQuery, UnusedBlocksSlice>();
            //_services.AddTransient<IContentSlice, UnusedBlocksSlice>();
            _services.AddSingleton<ISchemaDataMapper<BlogItemPage>, BlogItemPageSchemaMapper>();
            _services.AddSingleton<ISchemaDataMapper<HomePage>, HomePageSchemaMapper>();
            _services.AddSingleton<ISchemaDataMapper<LocationItemPage>, LocationItemPageSchemaDataMapper>();
            // Foundation.Features.Shared
            _services.AddSingleton<IMailService, MailService>();
            _services.AddSingleton<IHtmlDownloader, HtmlDownloader>();
        }

        public void Initialize(InitializationEngine context)
        {
            context.InitializeFoundationFindCms();
            context.InitComplete += ContextOnInitComplete;

            //SearchClient.Instance.Conventions.UnifiedSearchRegistry.ForInstanceOf<LocationListPage>()
            //    .ProjectImageUriFrom(page => new Uri(context.Locate.Advanced.GetInstance<UrlResolver>().GetUrl(page.PageImage), UriKind.Relative));

            //SearchClient.Instance.Conventions.ForInstancesOf<LocationItemPage>().IncludeField(dp => dp.TagString());
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.InitComplete -= ContextOnInitComplete;
        }

        private void ContextOnInitComplete(object sender, EventArgs eventArgs)
        {
            //var i = 0;
            //_services.AddTransient<ContentAreaRenderer, FoundationContentAreaRenderer>();          
        }
    }
}