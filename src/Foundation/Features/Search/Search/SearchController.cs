using Castle.Core.Internal;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Search.Search
{
    public class SearchController : PageController<SearchResultPage>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;
        private readonly ISearchService _cmsSearchService;

        public SearchController(ISearchViewModelFactory viewModelFactory,
            ISearchService cmsSearchService)
        {
            _viewModelFactory = viewModelFactory;
            _cmsSearchService = cmsSearchService;
        }

        [HttpGet]
        [HttpPost]
        public ActionResult Index(SearchResultPage currentPage, FilterOptionViewModel filterOptions)
        {
            if (filterOptions == null || filterOptions.Q.IsNullOrEmpty())
            {
                return Redirect(Url.ContentUrl(ContentReference.StartPage));
            }

            var selectedFacets = HttpContext.Request.Query["facets"];
            var viewModel = _viewModelFactory.Create<SearchViewModel<SearchResultPage>, SearchResultPage>(currentPage, selectedFacets, filterOptions);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult QuickSearch([FromQuery] string search = "")
        {
            var model = new SearchViewModel<SearchResultPage>();
            var contentResult = _cmsSearchService.SearchContent(new FilterOptionViewModel()
            {
                Q = search,
                PageSize = 5,
                Page = 1,
                IncludeImagesContent = true
            });
            model.ContentSearchResult = contentResult;

            return View("_QuickSearchContent", model);
        }

        public ActionResult Facet(SearchResultPage currentPage, FilterOptionViewModel viewModel) => PartialView("_Facet", viewModel);

        public class AssetPreloadLink
        {
            public enum AssetType
            {
                Unknown = 0,
                Script = 100,
                Style = 200,
                Image = 300,
                Auto = 400
            }

            private const string Format = "<{0}>; rel=preload; as={1}";
            public string Url { get; set; }
            public AssetType Type { get; set; }
            public bool NoPush { get; set; }

            public AssetPreloadLink()
            {
                Type = AssetType.Auto;
            }

            public AssetPreloadLink(AssetType type)
            {
                Type = type;
            }

            public override string ToString()
            {
                if (Type == AssetType.Auto)
                {
                    if (Url.EndsWith(".js"))
                    {
                        Type = AssetType.Script;
                    }
                    else if (Url.EndsWith(".css"))
                    {
                        Type = AssetType.Style;
                    }
                    else if (Url.EndsWith(".png") || Url.EndsWith(".jpg") || Url.EndsWith(".jpeg"))
                    {
                        Type = AssetType.Image;
                    }
                    else
                    {
                        Type = AssetType.Unknown;
                    }
                }

                if (Type != AssetType.Unknown)
                {
                    var output = string.Format(Format, Url, Type.ToString().ToLowerInvariant());
                    if (NoPush)
                    {
                        return output + "; nopush";
                    }
                    return output;
                }

                return string.Empty;
            }
        }
    }
}