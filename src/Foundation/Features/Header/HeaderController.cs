using EPiServer.Web.Routing;
using Foundation.Features.Home;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        public async Task<ActionResult> GetHeader(HomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return await Task.FromResult(PartialView("_Header", _headerViewModelFactory.CreateHeaderViewModel(content, homePage)));
        }

        public async Task<ActionResult> GetHeaderLogoOnly()
        {
            return await Task.FromResult(PartialView("_HeaderLogo", _headerViewModelFactory.CreateHeaderLogoViewModel()));
        }
    }
}