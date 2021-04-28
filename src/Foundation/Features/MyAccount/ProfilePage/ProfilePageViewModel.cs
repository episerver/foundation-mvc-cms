using Foundation.Infrastructure.Cms.Users;
using Foundation.Features.Shared;

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