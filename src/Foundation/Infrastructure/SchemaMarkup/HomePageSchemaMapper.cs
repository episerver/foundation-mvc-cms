﻿using EPiServer.Web;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Features.Home;
using Schema.NET;
using System;
using System.Linq;
using Foundation.Cms.Settings;
using Foundation.Features.Settings;

namespace Foundation.Infrastructure.SchemaMarkup
{
    
    /// <summary>
    /// Create Schema website and organization objects from HomePage
    /// </summary>
    public class HomePageSchemaMapper : ISchemaDataMapper<HomePage>
    {
        private readonly ISettingsService _settingsService;
        public HomePageSchemaMapper(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }
        public Thing Map(HomePage content)
        {
            var layoutSettings = _settingsService.GetSiteSettings<LayoutSettings>();

            return new WebSite
            {
                MainEntity = new Organization
                {
                    Name = layoutSettings?.CompanyName ?? content.Name,
                    Url = SiteDefinition.Current?.SiteUrl,
                    ContactPoint = new ContactPoint()
                    {
                        Email = layoutSettings?.CompanyEmail ?? new OneOrMany<string>(),
                        Telephone = layoutSettings?.CompanyPhone ?? new OneOrMany<string>()
                    },
                    SameAs = layoutSettings?.SocialLinks != null ? new OneOrMany<Uri>(layoutSettings?.SocialLinks.Select(x => new Uri(x.Href ?? string.Empty)).ToArray()) : new OneOrMany<Uri>()
                },
                Url = content.GetUri(true)
            };

        }
    }
}