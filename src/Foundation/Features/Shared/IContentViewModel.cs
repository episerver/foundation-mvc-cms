using EPiServer.Core;
using Foundation.Cms.Pages;
using System.Web;

namespace Foundation.Features.Shared
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        CmsHomePage StartPage { get; }
        HtmlString SchemaMarkup { get; }
    }
}