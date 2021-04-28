using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Web;
using EPiServer.Web;
using Foundation.Features.Home;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Foundation.Features.Preview
{
    /// <summary>
    /// Used to keep common code for the preview controllers
    /// </summary>
    public class PreviewControllerHelper
    {
        private readonly IContentLoader _contentLoader;
        private readonly TemplateResolver _templateResolver;
        private readonly DisplayOptions _displayOptions;

        public PreviewControllerHelper(IContentLoader contentLoader, TemplateResolver templateResolver, DisplayOptions displayOptions)
        {
            _contentLoader = contentLoader;
            _templateResolver = templateResolver;
            _displayOptions = displayOptions;
        }

        public ActionResult RenderResult(IContent currentContent, HttpContext httpContext)
        {
            //As the layout requires a page for title etc we "borrow" the start page
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);

            var model = new PreviewModel(startPage, currentContent);

            var supportedDisplayOptions = _displayOptions
                .Select(x => new
                {
                    x.Tag,
                    x.Name,
                    Supported = SupportsTag(currentContent, x.Tag, httpContext)
                }).ToList();

            if (!supportedDisplayOptions.Any(x => x.Supported))
            {
                return new ViewResult
                {
                    ViewName = "~/Features/Preview/Index.cshtml",
                    ViewData = { Model = model }
                };
            }
            foreach (var displayOption in supportedDisplayOptions)
            {
                var contentArea = new ContentArea();
                contentArea.Items.Add(new ContentAreaItem
                {
                    ContentLink = currentContent.ContentLink
                });
                var areaModel = new PreviewModel.PreviewArea
                {
                    Supported = displayOption.Supported,
                    AreaTag = displayOption.Tag,
                    AreaName = displayOption.Name,
                    ContentArea = contentArea
                };

                model.Areas.Add(areaModel);
            }

            return new ViewResult
            {
                ViewName = "~/Features/Preview/Index.cshtml",
                ViewData = { Model = model }
            };
        }

        private bool SupportsTag(IContent content, string tag, HttpContext httpContext)
        {
            var templateModel = _templateResolver.Resolve(httpContext,
                content.GetOriginalType(),
                content,
                TemplateTypeCategories.MvcPartial,
                tag);

            return templateModel != null;
        }

    }
}