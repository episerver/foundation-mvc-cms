using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.CustomPageFolder
{
    public class CustomPageController : PageController<CustomPage>
    {
        public ActionResult Index(CustomPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}