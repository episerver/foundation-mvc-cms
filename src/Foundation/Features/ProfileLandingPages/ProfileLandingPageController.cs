using EPiServer.Web.Mvc;
using Foundation.Features.LandingPages.ProfileLandingPages;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.ProfileLandingPages.ProfileLandingPage
{
    public class ProfileLandingPageController : PageController<ProfileLandingPage>
    {
        public ActionResult Index(ProfileLandingPage currentPage)
        {
            var model = ProfileLandingPageViewModel.Create(currentPage);
            return View(model);
        }
    }
}