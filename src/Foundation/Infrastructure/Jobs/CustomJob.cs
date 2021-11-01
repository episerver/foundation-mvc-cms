using EPiServer.Framework.Blobs;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using System.Text;
using EPiServer;
using Foundation.Features.CustomPageFolder;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Cms.Extensions;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Foundation.Infrastructure.Jobs
{
    [ScheduledPlugIn(DisplayName = "Custom job", Description = "Custom job", SortIndex = 10000)]
    [ServiceConfiguration]
    public class CustomJob : ScheduledJobBase
    {
        protected Injected<IBlobFactory> BlobFactory { get; set; }
        private int _count;
        private int _failCount;
        private readonly StringBuilder _errorText = new StringBuilder();
        private readonly IContentLoader _contentLoader;

        public CustomJob(IContentLoader contentLoader)
        {
            IsStoppable = false;
            _contentLoader = contentLoader;
        }

        public override string Execute()
        {
            Console.WriteLine("Start");
            var pages = _contentLoader.GetChildren<CustomPage>(ContentReference.RootPage);

            Console.WriteLine(pages);
            foreach (var page in pages)
            {
                Console.WriteLine("Page:" + page.Name, page.ContentLink);
            }
            
            return "";
        }
    }
}