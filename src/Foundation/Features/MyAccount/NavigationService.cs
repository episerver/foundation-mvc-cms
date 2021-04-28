using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Features.Header;
using Foundation.Features.Settings;

namespace Foundation.Features.MyAccount
{
    [ServiceConfiguration]
    public class NavigationService
    {
        private readonly ISettingsService _settingsService;

        public NavigationService(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public MyAccountNavigationViewModel MyAccountMenu()
        {
            var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
            var layoutsettings = _settingsService.GetSiteSettings<LayoutSettings>();
            if (referenceSettings == null || layoutsettings == null)
            {
                return null;
            }

            var model = new MyAccountNavigationViewModel
            {
                CurrentPageType = MyAccountPageType.Link,
                MenuItemCollection = new LinkItemCollection()
            };

            var menuItems = layoutsettings.MyAccountCmsMenu;
            if (menuItems == null)
            {
                return model;
            }

            menuItems = menuItems.CreateWritableClone();
            model.MenuItemCollection.AddRange(menuItems);
            return model;
        }
    }
}