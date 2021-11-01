using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Features.Shared.SelectionFactories;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.NpmPackageViewerBlock
{
    [ContentType(DisplayName = "Npm Package Viewer Block",
        GUID = "8bdfac92-3dbd-43b9-a092-522bd67ee8b3",
        Description = "NPM package viewer block",
        GroupName = GroupNames.Content)]
    public class NpmPackageViewerBlock : FoundationBlockData//, IDashboardItem
    {
        [Required]
        [Display(Name = "Package name")]
        public virtual string PackageName { get; set; }

        //public void SetItem(ItemModel itemModel)
        //{
        //    itemModel.Description = Callout?.CalloutContent.ToHtmlString();
        //    itemModel.Image = BackgroundImage;
        //}
    }
}