using System;
using ImageTransform.Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace ImageTransform.Api.ModelBinders
{
    public class FilterModelBinderProvider: IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(FilterModel))
            {
                return new BinderTypeModelBinder(typeof(FilterModelBinder));
            }

            return null;
        }
    }
}
