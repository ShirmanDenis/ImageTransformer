using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kontur.ImageTransformer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Kontur.ImageTransformer.ModelBinders
{
    public class FilterModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            
            var filterModel = new FilterModel();
            bindingContext.Result = ModelBindingResult.Success(filterModel);

            return Task.CompletedTask;
        }
    }
}
