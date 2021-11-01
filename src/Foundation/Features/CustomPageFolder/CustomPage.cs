using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.CustomPageFolder
{
    [ContentType(DisplayName = "Custom page 668",
       GUID = "DBED4258-8213-48DB-A11F-99C034182454",
       Description = "My custom page",
       GroupName = GroupNames.Content)]
    public class CustomPage : PageData
    {
        [Display(Name = "Heading", GroupName = SystemTabNames.Content, Order = 5)]
        public virtual string Heading { get; set; }

        [CultureSpecific]
        [Display(Name = "Main body", GroupName = SystemTabNames.Content, Order = 100)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 90)]
        public virtual ContentArea MainContentArea { get; set; }
        
        [CultureSpecific]
        [Display(Name = "BgImage", GroupName = SystemTabNames.Content, Order = 240)]
        public virtual ContentReference BackgroundImage { get; set; }

        [CultureSpecific]
        [Display(Name = "Background video", GroupName = SystemTabNames.Content, Order = 250)]
        public virtual ContentReference BackgroundVideo { get; set; }
        
        [Display(Name = "Publish counter", GroupName = SystemTabNames.Content, Order = 900)]
        [CultureSpecific]
        [ScaffoldColumn(false)]
        public virtual  string PublishCounterValue { get; set; }
        
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            PublishCounterValue = "0";
        }
    }
}