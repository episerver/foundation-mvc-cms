using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Features.Settings;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Cms.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.MyAccount.ProfilePage
{
    [Authorize]
    public class ProfilePageController : IdentityControllerBase<ProfilePage>
    {
        private readonly ISettingsService _settingsService;

        public ProfilePageController(
            ApplicationSignInManager<SiteUser> signinManager,
            ApplicationUserManager<SiteUser> userManager,
            ISettingsService settingsService) : base(signinManager, userManager)
        {
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Index(ProfilePage currentPage)
        {
            var currentUser = await UserManager.GetUserAsync(User);
            var viewModel = new ProfilePageViewModel(currentPage)
            {
                CurrentUser = currentUser,
                ResetPasswordPage = UrlResolver.Current.GetUrl(_settingsService.GetSiteSettings<ReferencePageSettings>()?.ResetPasswordPage ?? ContentReference.StartPage)
            };

            return View(viewModel);
        }
    }
}