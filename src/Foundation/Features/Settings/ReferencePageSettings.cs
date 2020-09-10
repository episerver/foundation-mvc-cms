using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Cms.Settings;
using Foundation.Features.MyAccount.ResetPassword;
using Foundation.Features.Search.Search;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Site Structure Settings Page",
        GUID = "bf69f959-c91b-46cb-9829-2ecf9d11e13b",
        Description = "Site structure settings",
        SettingsName = "Page references")]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-structure-settings.png")]
    public class ReferencePageSettings : SettingsBase
    {
        #region Site Structure

        [CultureSpecific]
        [AllowedTypes(typeof(SearchResultPage))]
        [Display(Name = "Search page", GroupName = TabNames.SiteStructure, Order = 10)]
        public virtual ContentReference SearchPage { get; set; }

        [CultureSpecific]
        [Display(Name = "Store locator page", GroupName = TabNames.SiteStructure, Order = 20)]
        public virtual ContentReference StoreLocatorPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(ResetPasswordPage))]
        [Display(Name = "Reset password page", GroupName = TabNames.SiteStructure, Order = 40)]
        public virtual ContentReference ResetPasswordPage { get; set; }

        [CultureSpecific]
        [Display(Name = "Resource not found page", GroupName = TabNames.SiteStructure, Order = 180)]
        public virtual ContentReference PageNotFound { get; set; }

        #endregion

        #region Mail templates

        [CultureSpecific]
        [Display(Name = "Send order confirmations", GroupName = TabNames.MailTemplates, Order = 10)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(ResetPasswordMailPage))]
        [Display(Name = "Reset password", GroupName = TabNames.MailTemplates, Order = 30)]
        public virtual ContentReference ResetPasswordMail { get; set; }

        #endregion
    }
}