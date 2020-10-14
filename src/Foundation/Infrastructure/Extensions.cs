﻿using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Foundation.Features.Folder;
using Foundation.Features.Home;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Find;
using EPiServer.Logging;
using EPiServer.Scheduler;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Settings;

namespace Foundation.Infrastructure
{
    public static class Extensions
    {
        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IUrlResolver> _urlResolver =
            new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<ISiteDefinitionRepository> _siteDefinitionRepository =
            new Lazy<ISiteDefinitionRepository>(() => ServiceLocator.Current.GetInstance<ISiteDefinitionRepository>());

        public static ContentReference GetRelativeStartPage(this IContent content)
        {
            if (content is HomePage)
            {
                return content.ContentLink;
            }

            var ancestors = _contentLoader.Value.GetAncestors(content.ContentLink);
            var startPage = ancestors.FirstOrDefault(x => x is HomePage) as HomePage;
            return startPage == null ? ContentReference.StartPage : startPage.ContentLink;
        }

        public static IEnumerable<T> FindPagesRecursively<T>(this IContentLoader contentLoader, PageReference pageLink) where T : PageData
        {
            foreach (var child in contentLoader.GetChildren<T>(pageLink))
            {
                yield return child;
            }

            foreach (var folder in contentLoader.GetChildren<FolderPage>(pageLink))
            {
                foreach (var nestedChild in contentLoader.FindPagesRecursively<T>(folder.PageLink))
                {
                    yield return nestedChild;
                }
            }
        }

        public static void InstallDefaultContent()
        {
            if (_siteDefinitionRepository.Value.List().Any() || Type.GetType("Foundation.Features.Setup.SetupController, Foundation") != null)
            {
                return;
            }

            var urlBuilder = new UrlBuilder(HttpContext.Current.Request.Url);
            urlBuilder.Path = WebHostingEnvironment.Instance.WebRootVirtualPath;

            var siteDefinition = new SiteDefinition
            {
                Name =  HostingEnvironment.SiteName,
                SiteUrl = new Uri(VirtualPathUtility.AppendTrailingSlash((string)urlBuilder)),
                Hosts = new List<HostDefinition>()
                {
                    new HostDefinition()
                    {
                        Name = "*", 
                        Type = HostDefinitionType.Undefined
                    },
                    new HostDefinition()
                    {
                        Name = urlBuilder.Uri.Authority,
                        Type = HostDefinitionType.Primary
                    },
                    new HostDefinition()
                    {
                        Name = HostDefinition.WildcardHostName
                    }
                }
            };

            CreateSite(new FileStream(
                    HostingEnvironment.MapPath("~/App_Data/foundation.episerverdata") ?? throw new InvalidOperationException(),
                    FileMode.Open, 
                    FileAccess.Read, 
                    FileShare.Read),
                siteDefinition,
                ContentReference.RootPage);

            var config = Configuration.GetConfiguration();
            if (!config.ServiceUrl.Equals("https://es-us-api01.episerver.com/9IKGqgMZaTD9KP4Op3ygsVB6JeJzR0N6") && !config.DefaultIndex.Equals("episerverab_index55794"))
            {
                RunIndexJob(ServiceLocator.Current.GetInstance<IScheduledJobExecutor>(),
                    ServiceLocator.Current.GetInstance<IScheduledJobRepository>(), new Guid("8EB257F9-FF22-40EC-9958-C1C5BA8C2A53"));
            }
        }

        private static void RunIndexJob(IScheduledJobExecutor scheduledJobExecutor, IScheduledJobRepository scheduledJobRepository, Guid jobId)
        {
            var job = scheduledJobRepository.Get(jobId);
            if (job == null)
            {
                return;
            }

            scheduledJobExecutor.StartAsync(job, new JobExecutionOptions { Trigger = ScheduledJobTrigger.User });
        }

        private static void CreateSite(Stream stream, SiteDefinition siteDefinition, ContentReference startPage)
        {
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = false;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = false;
            ImportEpiserverContent(stream, startPage, ServiceLocator.Current.GetInstance<IDataImporter>(), siteDefinition);
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = true;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = true;
        }

        public static bool ImportEpiserverContent(Stream stream,
            ContentReference destinationRoot,
            IDataImporter importer,
            SiteDefinition siteDefinition = null)
        {
            var success = false;
            try
            {
                var log = importer.Import(stream, destinationRoot, new ImportOptions
                {
                    KeepIdentity = true,
                    EnsureContentNameUniqueness = false
                });

                var status = importer.Status;

                if (status == null)
                {
                    return false;
                }

                UpdateLanguageBranches(status);
                if (siteDefinition != null && !ContentReference.IsNullOrEmpty(status.ImportedRoot))
                {
                    siteDefinition.StartPage = status.ImportedRoot;
                    _siteDefinitionRepository.Value.Save(siteDefinition);
                    SiteDefinition.Current = siteDefinition;
                    success = true;
                }
            }
            catch (Exception exception)
            {
                LogManager.GetLogger().Error(exception.Message, exception);
                success = false;
            }

            return success;
        }

        private static void UpdateLanguageBranches(IImportStatus status)
        {
            var languageBranchRepository = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>();

            if (status.ContentLanguages == null)
            {
                return;
            }

            foreach (var languageId in status.ContentLanguages)
            {
                var languageBranch = languageBranchRepository.Load(languageId);

                if (languageBranch == null)
                {
                    languageBranch = new LanguageBranch(languageId);
                    languageBranchRepository.Save(languageBranch);
                }
                else if (!languageBranch.Enabled)
                {
                    languageBranch = languageBranch.CreateWritableClone();
                    languageBranch.Enabled = true;
                    languageBranchRepository.Save(languageBranch);
                }
            }
        }
    }
}