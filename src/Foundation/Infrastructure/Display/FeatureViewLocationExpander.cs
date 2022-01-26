using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure.Display
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        private const string Feature = "feature";
        private readonly List<string>  _viewLocationFormats = new List<string>() 
        {
            "/Features/Shared/{0}.cshtml",
            "/Features/Blocks/{0}.cshtml",
            "/Features/Blocks/{1}/{0}.cshtml",
            "/Features/Shared/Views/{0}.cshtml",
            "/Features/Shared/Views/{1}/{0}.cshtml",
            "/Features/Shared/Views/Header/{0}.cshtml",
            "/Cms/Views/{1}/{0}.cshtml",
            "/Features/{3}/{1}/{0}.cshtml",
            "/Features/{3}/{0}.cshtml",
            "/Features/Shared/Views/ElementBlocks/{0}.cshtml"
        };
            
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (viewLocations == null)
            {
                throw new ArgumentNullException(nameof(viewLocations));
            }

            var controllerActionDescriptor = context.ActionContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null && controllerActionDescriptor.Properties.ContainsKey("feature"))
            {
                string featureName = controllerActionDescriptor.Properties[Feature] as string;
                foreach (var item in ExpandViewLocations(_viewLocationFormats.Union(viewLocations), featureName))
                {
                    yield return item;
                }
            }
            else
            {
                foreach (var location in viewLocations)
                {
                    yield return location;
                }
            }
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controllerActionDescriptor = context.ActionContext?.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null || !controllerActionDescriptor.Properties.ContainsKey(Feature))
            {
                return;
            }
            context.Values[Feature] = controllerActionDescriptor?.Properties[Feature].ToString();
        }

        private IEnumerable<string> ExpandViewLocations(IEnumerable<string> viewLocations, string featureName)
        {
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }
    }
}