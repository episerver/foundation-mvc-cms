using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using System.Web.Routing;
using EPiServer;
using EPiServer.ContentApi.Core.ContentResult;
using EPiServer.ContentApi.Core.Internal;
using EPiServer.ContentApi.Core.Security.Internal;
using EPiServer.ContentApi.Core.Serialization;
using EPiServer.ContentApi.Core.Serialization.Models;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web;

namespace Foundation.Cms.Settings
{
    [RoutePrefix("api/foundation/v1/settings")]
    [ContentApiAuthorization]
    [ContentApiCors]
    [CorsOptionsActionFilter]
    public class SettingsApiController : ApiController
    {
        protected virtual ISettingsService SettingsService { get; private set; }
        protected virtual IContentModelMapper ContentModelMapper { get; private set; }
        protected virtual ContentResultService ContentResultService { get; private set; }
        protected virtual ISiteDefinitionResolver SiteDefinitionResolver { get; private set; }

        public SettingsApiController(
            ISettingsService settingsService,
            IContentModelMapper contentModelMapper,
            ContentResultService contentResultService,
            ISiteDefinitionResolver siteDefinitionResolver
        ) {
            SettingsService = settingsService;
            ContentModelMapper = contentModelMapper;
            ContentResultService = contentResultService;
            SiteDefinitionResolver = siteDefinitionResolver;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult Ping()
        {
            return Ok("Settings Service available");
        }

        [Route("current")]
        [HttpGet]
        public IHttpActionResult ListCurrentSettings([ValueProvider(typeof(AcceptLanguageHeaderValueProviderFactory))] List<string> languages)
        {
            var siteId = ResolveSiteId();
            return siteId != null && siteId != Guid.Empty ? ListSettings(siteId, languages) : NotFound();
        }

        [Route("{siteId}")]
        [HttpGet]
        public IHttpActionResult ListSettings(Guid siteId, [ValueProvider(typeof(AcceptLanguageHeaderValueProviderFactory))] List<string> languages)
        {
            var language = languages.DefaultIfEmpty(ContentLanguage.PreferredCulture.Name).FirstOrDefault();
            var siteSettings = GetSiteSettings(siteId, language).Where(x => x.Value is SettingsBase contentData && contentData.Language.Name.Equals(language, StringComparison.OrdinalIgnoreCase)).Select(x => x.Key.Name);
            return Ok(new SettingsContainerList()
            {
                SiteId = siteId,
                Language = language,
                Containers = siteSettings
            });
        }

        [Route("current/{group}")]
        [HttpGet]
        public IHttpActionResult GetCurrentSettings([ValueProvider(typeof(AcceptLanguageHeaderValueProviderFactory))] List<string> languages, string group, string expand = "")
        {
            var siteId = ResolveSiteId();
            return siteId != null && siteId != Guid.Empty ? GetSettings(siteId, languages, group, expand) : NotFound();
        }

        [Route("{siteId}/{group}")]
        [HttpGet]
        public IHttpActionResult GetSettings(Guid siteId, [ValueProvider(typeof(AcceptLanguageHeaderValueProviderFactory))] List<string> languages, string group, string expand = "")
        {
            var language = languages.DefaultIfEmpty(ContentLanguage.PreferredCulture.Name).FirstOrDefault();
            var settingGroups = GetSiteSettings(siteId, language).Where(x => x.Key.Name.Equals(group) && x.Value is SettingsBase settingsBase && settingsBase.Language.Name.Equals(language, StringComparison.OrdinalIgnoreCase));
            if (settingGroups.Count() == 0)
                return NotFound();

            if (settingGroups.Count() > 1)
                throw new Exception("Multiple setting groups match this name");

            if (!(settingGroups.First().Value is SettingsBase contentData))
                return NotFound();

            var contentModel = ContentModelMapper.TransformContent(contentData, true, expand);
            return new EPiServer.ContentApi.Core.ContentResult.Internal.ContentApiResult<ContentApiModel>(contentModel, HttpStatusCode.OK);
        }

        protected Dictionary<Type, object> GetSiteSettings(Guid siteId, string contentLanguage )
        {
            if (string.IsNullOrWhiteSpace(contentLanguage))
                contentLanguage = ContentLanguage.PreferredCulture.Name;
            if (siteId == null || siteId == Guid.Empty)
                return default;

            var txtSiteId = siteId.ToString();
            var modeId = PageEditing.PageIsInEditMode ? "-common-draft-" : "-";

            return (new string[] { $"{txtSiteId}{modeId}{contentLanguage}", $"{txtSiteId}{modeId}default" }).Select(x => SettingsService.SiteSettings.TryGetValue(x, out var siteSettings) ? siteSettings : default).Where(x => x != null).FirstOrDefault();
        }

        protected Guid ResolveSiteId()
        {
            var site = SiteDefinitionResolver.GetByHostname(Request.RequestUri.Host, true);
            return site?.Id ?? Guid.Empty;
        }


    }

    public class SettingsContainerList
    {
        public Guid SiteId { get; set; }
        public string Language { get; set; }
        public IEnumerable<string> Containers { get; set; }
    }
}
