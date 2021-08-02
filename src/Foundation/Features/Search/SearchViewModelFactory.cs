using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Statistics;
using EPiServer.Framework.Localization;
using Foundation.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Foundation.Features.Search
{
    public interface ISearchViewModelFactory
    {
        SearchViewModel<TContent> Create<TModel, TContent>(TContent currentContent,
            string selectedFacets, FilterOptionViewModel filterOption)
            where TContent : IContent;
    }

    public class SearchViewModelFactory : ISearchViewModelFactory
    {
        private readonly ISearchService _searchService;
        private readonly LocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextBase;
        private readonly IClient _findClient;

        public SearchViewModelFactory(LocalizationService localizationService,
            ISearchService searchService,
            IHttpContextAccessor httpContextBase,
            IClient findClient)
        {
            _searchService = searchService;
            _httpContextBase = httpContextBase;
            _localizationService = localizationService;
            _findClient = findClient;
        }

        public SearchViewModel<TContent> Create<TModel, TContent>(TContent currentContent, string selectedFacets, FilterOptionViewModel filterOption)
            where TContent : IContent
        {
            var model = new SearchViewModel<TContent>(currentContent);

            if (filterOption.Q != null && (filterOption.Q.StartsWith("*") || filterOption.Q.StartsWith("?")))
            {
                model.CurrentContent = currentContent;
                model.FilterOption = filterOption;
                model.HasError = true;
                model.ErrorMessage = _localizationService.GetString("/Search/BadFirstCharacter");

                return model;
            }

            model.ContentSearchResult = _searchService.SearchContent(filterOption);
            model.CurrentContent = currentContent;
            model.FilterOption = filterOption;
            model.Query = filterOption.Q;
            model.IsMobile = _httpContextBase.HttpContext?.Request.Headers.IsMobile() ?? false;
            model.DidYouMeans = string.IsNullOrEmpty(model.FilterOption.Q) ? null : model.ContentSearchResult.Hits.Any() ? null : _findClient.Statistics().GetDidYouMean(model.FilterOption.Q);

            return model;
        }
    }
}
