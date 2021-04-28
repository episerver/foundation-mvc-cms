using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Infrastructure.Cms.Extensions;
using Foundation.Features.Events.CalendarEvent;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Events.CalendarBlock
{
    public class CalendarBlockController : BlockComponent<CalendarBlock>
    {
        private readonly IContentLoader _contentLoader;

        public CalendarBlockController(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public override IViewComponentResult Invoke(CalendarBlock currentBlock)
        {
            var model = new CalendarBlockViewModel(currentBlock);

            return View(model);
        }

        private IEnumerable<CalendarEventPage> GetEvents(int blockId)
        {
            var currentBlock = _contentLoader.Get<CalendarBlock>(new ContentReference(blockId));
            IEnumerable<CalendarEventPage> events;
            var root = currentBlock.EventsRoot;
            if (currentBlock.Recursive)
            {
                events = root.GetAllRecursively<CalendarEventPage>();
            }
            else
            {
                events = _contentLoader.GetChildren<CalendarEventPage>(root);
            }

            if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
            {
                events = events.Where(x => x.Category.Intersect(currentBlock.CategoryFilter).Any());
            }

            events.Take(currentBlock.Count);

            return events;
        }

        [HttpPost]
        public ContentResult CalendarEvents(int blockId)
        {
            var events = GetEvents(blockId);
            var result = events.Select(x => new
            {
                title = x.Name,
                start = x.EventStartDate,
                end = x.EventEndDate,
                url = x.LinkURL
            });

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }

        [HttpPost]
        public ContentResult UpcomingEvents(int blockId)
        {
            var events = GetEvents(blockId);
            var result = events.Where(x => x.EventStartDate >= DateTime.Now)
                .OrderBy(x => x.EventStartDate)
                .Select(x => new
                {
                    x.Name,
                    x.EventStartDate,
                    x.EventEndDate,
                    x.LinkURL
                });

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(result),
                ContentType = "application/json",
            };
        }
    }
}