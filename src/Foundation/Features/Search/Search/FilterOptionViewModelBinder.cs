using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace Foundation.Features.Search
{
    public class FilterOptionViewModelBinder : IModelBinder
    {
        private readonly IContentLoader _contentLoader;

        public FilterOptionViewModelBinder(IContentLoader contentLoader) => _contentLoader = contentLoader;

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var modelName = bindingContext.ModelName = "FilterOption";

            // Try to fetch the value of the argument by name
            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            // Check if the argument value is null or empty
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            var contentLink = bindingContext.ActionContext.HttpContext.GetContentLink();
            IContent content = null;
            if (!ContentReference.IsNullOrEmpty(contentLink))
            {
                content = _contentLoader.Get<IContent>(contentLink);
            }

            var query = bindingContext.ActionContext.HttpContext.Request.Query["search"];
            var sort = bindingContext.ActionContext.HttpContext.Request.Query["sort"];
            var section = bindingContext.ActionContext.HttpContext.Request.Query["t"];
            var page = bindingContext.ActionContext.HttpContext.Request.Query["p"];
            var confidence = bindingContext.ActionContext.HttpContext.Request.Query["confidence"];

            var model = new FilterOptionViewModel();
            SetupModel(model, query, sort, section, page, content, confidence);
            bindingContext.Result = ModelBindingResult.Success(model);
            await Task.CompletedTask;
        }

        protected virtual void SetupModel(FilterOptionViewModel model, string q, string sort, string section, string page, IContent content, string confidence)
        {
            EnsurePage(model, page);
            EnsureQ(model, q);
            EnsureSort(model, sort);
            EnsureSection(model, section);
            model.Confidence = decimal.TryParse(confidence, out var confidencePercentage) ? confidencePercentage : 0;
        }

        protected virtual void EnsurePage(FilterOptionViewModel model, string page)
        {
            if (model.Page < 1)
            {
                if (!string.IsNullOrEmpty(page))
                {
                    model.Page = int.Parse(page);
                }
                else
                {
                    model.Page = 1;
                }
            }
        }

        protected virtual void EnsureQ(FilterOptionViewModel model, string q)
        {
            if (string.IsNullOrEmpty(model.Q))
            {
                model.Q = q;
            }
        }

        protected virtual void EnsureSection(FilterOptionViewModel model, string section)
        {
            if (string.IsNullOrEmpty(model.SectionFilter))
            {
                model.SectionFilter = section;
            }
        }

        protected virtual void EnsureSort(FilterOptionViewModel model, string sort)
        {
            if (string.IsNullOrEmpty(model.Sort))
            {
                model.Sort = sort;
            }
        }
    }
}
