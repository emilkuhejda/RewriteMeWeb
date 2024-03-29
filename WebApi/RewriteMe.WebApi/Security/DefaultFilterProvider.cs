﻿using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RewriteMe.WebApi.Security
{
    internal abstract class DefaultFilterProvider : IFilterProvider
    {
        public int Order => -1000;

        public void OnProvidersExecuting(FilterProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (context.ActionContext.ActionDescriptor.FilterDescriptors == null)
                return;

            for (var index = 0; index < context.Results.Count; ++index)
            {
                ProvideFilter(context, context.Results[index]);
            }
        }

        public void OnProvidersExecuted(FilterProviderContext context)
        {
        }

        public virtual void ProvideFilter(FilterProviderContext context, FilterItem filterItem)
        {
            if (filterItem.Filter != null)
                return;

            var filter = filterItem.Descriptor.Filter;
            var filterFactory = filter as IFilterFactory;
            if (filterFactory == null)
            {
                filterItem.Filter = filter;
                filterItem.IsReusable = true;
            }
            else
            {
                var requestServices = context.ActionContext.HttpContext.RequestServices;
                filterItem.Filter = filterFactory.CreateInstance(requestServices);
                filterItem.IsReusable = filterFactory.IsReusable;
                if (filterItem.Filter == null)
                    throw new InvalidOperationException(typeof(IFilterFactory).Name);

                ApplyFilterToContainer(filterItem.Filter, filterFactory);
            }
        }

        private void ApplyFilterToContainer(object actualFilter, IFilterMetadata filterMetadata)
        {
            var filterContainer = actualFilter as IFilterContainer;
            if (filterContainer == null)
                return;

            filterContainer.FilterDefinition = filterMetadata;
        }
    }
}
