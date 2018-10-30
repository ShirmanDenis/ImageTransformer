﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.ImageTransformer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Kontur.ImageTransformer.ModelBinders
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
