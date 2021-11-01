using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Media
{
    [ContentType(DisplayName = "Custom Media Data",
        GUID = "20644be7-3c42-4f84-b893-ee021b73ce6c",
        Description = "Used for custom file types such as xxe, xxr, xxt")]
    [MediaDescriptor(ExtensionString = "xxe, xxr, xxt")]
    public class CustomMediaData : MediaData//, IDashboardItem
    {

        [Editable(false)]
        [Display(Name = "File size", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string FileSize { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 150)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Description of the image", GroupName = SystemTabNames.Content, Order = 160)]
        public virtual string Description { get; set; }
    }
}