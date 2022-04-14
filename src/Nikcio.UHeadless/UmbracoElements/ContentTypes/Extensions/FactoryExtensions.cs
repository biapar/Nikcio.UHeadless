﻿using Microsoft.Extensions.DependencyInjection;
using Nikcio.UHeadless.UmbracoElements.ContentTypes.Factories;

namespace Nikcio.UHeadless.UmbracoElements.ContentTypes.Extensions
{
    /// <inheritdoc/>
    public static class FactoryExtensions
    {
        /// <summary>
        /// Adds factories
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IContentTypeFactory<>), typeof(ContentTypeFactory<>));

            return services;
        }
    }
}
