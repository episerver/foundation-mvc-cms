using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Foundation.Features.Search
{
    public class FindModelBinderProvider : IModelBinderProvider
    {
        private static readonly IDictionary<Type, Type> ModelBinderTypeMappings = new Dictionary<Type, Type>
        {
            {typeof(FilterOptionViewModel), typeof(FilterOptionViewModelBinder)},
        };

        public IModelBinder GetBinder(ModelBinderProviderContext modelBinderProviderContext)
        {
            if (ModelBinderTypeMappings.ContainsKey(modelBinderProviderContext.Metadata.ModelType))
            {
                return ServiceLocator.Current.GetService(ModelBinderTypeMappings[modelBinderProviderContext.Metadata.ModelType]) as IModelBinder;
            }
            return null;
        }
    }
}