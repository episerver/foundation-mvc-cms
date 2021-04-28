using EPiServer.Web.Routing;
using Foundation.Features.Home;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Header
{
    public class HeaderController : Controller
    {
        private readonly IHeaderViewModelFactory _headerViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;

        public HeaderController(IHeaderViewModelFactory headerViewModelFactory,
            IContentRouteHelper contentRouteHelper)
        {
            _headerViewModelFactory = headerViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
        }

        public ActionResult GetHeader(HomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return PartialView("_Header", _headerViewModelFactory.CreateHeaderViewModel(content, homePage));
        }

        public ActionResult GetHeaderLogoOnly()
        {
            return PartialView("_HeaderLogo", _headerViewModelFactory.CreateHeaderLogoViewModel());
        }
    }
}