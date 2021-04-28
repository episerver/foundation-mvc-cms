using EPiServer;
using EPiServer.Core;
using Foundation.Features.Home;
using Foundation.Infrastructure.Attributes;
using Foundation.Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Login
{
    public class UserController : Controller
    {
        private readonly IContentLoader _contentLoader;

        public UserController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        [OnlyAnonymous]
        public ActionResult Index(string returnUrl)
        {
            return View(Url.GetUserViewModel(returnUrl));
        }

        [OnlyAnonymous]
        public ActionResult Register()
        {
            var model = new UserViewModel();
            var homePage = _contentLoader.Get<PageData>(ContentReference.StartPage) as HomePage;
            // model.Logo = Url.ContentUrl(homePage?.SiteLogo);
            model.Title = "Register";
            return View(model);
        }
    }
}