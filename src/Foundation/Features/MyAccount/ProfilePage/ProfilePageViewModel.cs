using Foundation.Features.Shared;
using Foundation.Infrastructure.Cms.Users;

namespace Foundation.Features.MyAccount.ProfilePage
{
    public class ProfilePageViewModel : ContentViewModel<ProfilePage>
    {
        public ProfilePageViewModel()
        {
        }

        public ProfilePageViewModel(ProfilePage profilePage) : base(profilePage)
        {
        }
        public SiteUser CurrentUser { get; set; }
        public string ResetPasswordPage { get; set; }
    }
}