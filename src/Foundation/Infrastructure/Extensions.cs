using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Foundation.Features.Home;
using System;
using System.Linq;

namespace Foundation.Infrastructure
{
    public static class Extensions
    {
        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

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
    }
}