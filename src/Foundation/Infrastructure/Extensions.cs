using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Features.Folder;
using Foundation.Features.Home;
using Foundation.Infrastructure.Cms.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure
{
    public static class Extensions
    {
        private static readonly Lazy<IContentRepository> _contentRepository =
            new Lazy<IContentRepository>(() => ServiceLocator.Current.GetInstance<IContentRepository>());

        private static readonly Lazy<IUrlResolver> _urlResolver =
            new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        public static ContentReference GetRelativeStartPage(this IContent content)
        {
            if (content is HomePage)
            {
                return content.ContentLink;
            }

            var ancestors = _contentRepository.Value.GetAncestors(content.ContentLink);
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

        public static string GetPublicUrl(this ContentReference contentLink, string language = null)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return string.Empty;
            }

            var content = language != null ? contentLink.Get<IContent>(language) : contentLink.Get<IContent>();
            if (content == null || !PublishedStateAssessor.IsPublished(content))
            {
                return string.Empty;
            }

            return _urlResolver.Value.GetUrl(contentLink, language);
        }


        public static string GetPublicUrl(this Guid contentGuid, string language)
        {
            var contentLink = PermanentLinkUtility.FindContentReference(contentGuid);
            return GetPublicUrl(contentLink, language);
        }
    }
}