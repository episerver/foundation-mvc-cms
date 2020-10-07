using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EPiServer.PlugIn;
using EPiServer.Shell.ObjectEditing;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Foundation.Cms;
using Foundation.Features.Blocks.MenuItemBlock;
using Foundation.Features.MyAccount.ResetPassword;
using Foundation.Features.Search.Search;
using Foundation.Features.Shared;
using Foundation.Features.Shared.SelectionFactories;
using Foundation.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Home
{
    [ContentType(DisplayName = "Cms Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true,
        GroupName = GroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/pages/home.png")]
    public class HomePage : FoundationPageData
    {
        [CultureSpecific]
        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual ContentArea TopContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Bottom content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea BottomContentArea { get; set; }

    }

    [PropertyDefinitionTypePlugIn]
    public class SelectionItemProperty : PropertyList<SelectionItem>
    {
    }
}