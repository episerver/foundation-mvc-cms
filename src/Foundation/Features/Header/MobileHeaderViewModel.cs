using EPiServer.SpecializedProperties;

namespace Foundation.Cms.ViewModels.Header
{
    public class MobileHeaderViewModel
    {
        public MegaMenuModel MenuModel { get; set; }

        public LinkItemCollection Pages { get; set; }

        public CmsHomePage StartPage { get; set; }
    }
}
