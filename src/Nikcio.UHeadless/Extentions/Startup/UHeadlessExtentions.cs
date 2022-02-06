﻿using HotChocolate.Configuration;
using HotChocolate.Execution.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nikcio.UHeadless.Queries;
using Nikcio.UHeadless.Reflection.Extentions;
using Nikcio.UHeadless.UmbracoContent.Content.Extentions;
using Nikcio.UHeadless.UmbracoContent.Content.Models;
using Nikcio.UHeadless.UmbracoContent.Content.Queries;
using Nikcio.UHeadless.UmbracoContent.ContentType.Models;
using Nikcio.UHeadless.UmbracoContent.Elements.Models;
using Nikcio.UHeadless.UmbracoContent.Properties.Extentions;
using Nikcio.UHeadless.UmbracoContent.Properties.Maps;
using Nikcio.UHeadless.UmbracoContent.Properties.Models;
using Nikcio.UHeadless.UmbracoContent.Properties.Queries;
using System;
using System.Collections.Generic;
using System.Reflection;
using Umbraco.Cms.Core.DependencyInjection;

namespace Nikcio.UHeadless.Extentions.Startup
{
    public static class UHeadlessExtentions
    {
        public static IUmbracoBuilder AddUHeadless(this IUmbracoBuilder builder,
                                                   List<Assembly> automapperAssemblies = null,
                                                   List<Action<IPropertyMap>> customPropertyMappings = null,
                                                   bool throwOnSchemaError = false,
                                                   TracingOptions tracingOptions = null)
        {
            builder.Services.AddUHeadlessAutomapper(automapperAssemblies);

            builder.Services
                .AddReflectionServices()
                .AddContentServices()
                .AddPropertyServices(customPropertyMappings);

            builder.Services
                .AddGraphQLServer()
                .AddUHeadlessGraphQL(throwOnSchemaError)
                .AddTracing(tracingOptions);


            return builder;
        }

        private static IRequestExecutorBuilder AddTracing(this IRequestExecutorBuilder requestExecutorBuilder, TracingOptions tracingOptions)
        {
            if (tracingOptions != null)
            {
                requestExecutorBuilder
                    .AddApolloTracing(tracingOptions.TracingPreference, tracingOptions.TimestampProvider);
            }
            return requestExecutorBuilder;
        }

        private static IUmbracoBuilder AddPropertyMapSettings(this IUmbracoBuilder builder, List<Action<IPropertyMap>> customPropertyMappings)
        {
            var propertyMap = new PropertyMap();

            builder.Services
                .AddSingleton<IPropertyMap>(propertyMap);

            if (customPropertyMappings != null)
            {
                foreach (var customPropertyMapping in customPropertyMappings)
                {
                    customPropertyMapping.Invoke(propertyMap);
                }
            }

            propertyMap.AddPropertyMapDefaults();

            return builder;
        }

        private static IServiceCollection AddUHeadlessAutomapper(this IServiceCollection services, List<Assembly> automapperAssemblies)
        {
            if (automapperAssemblies == null)
            {
                automapperAssemblies = new List<Assembly>();
            }

            automapperAssemblies.Add(typeof(UHeadlessExtentions).Assembly);

            services
                .AddAutoMapper(automapperAssemblies);

            return services;
        }

        public static IRequestExecutorBuilder AddUHeadlessGraphQL(this IRequestExecutorBuilder requestExecutorBuilder, bool throwOnSchemaError = false)
        {
            requestExecutorBuilder
                .InitializeOnStartup()
                .OnSchemaError(new OnSchemaError((dc, ex) =>
                {
                    var logger = dc.Services.GetService<ILogger<Query>>();
                    logger.LogError(ex, "Schema failed to generate. GraphQL is unavalible");
                    if (throwOnSchemaError)
                    {
                        throw ex;
                    }
                }))
                .AddQueryType<Query>()
                .AddTypeExtension<ContentQuery>()
                .AddTypeExtension<PropertyQuery>()
                .AddFiltering()
                .AddSorting()
                .AddInterfaceType<IPublishedContentGraphType>()
                .AddInterfaceType<IPublishedElementGraphType>()
                .AddType<PublishedContentGraphType>()
                .AddType<PublishedContentTypeGraphType>()
                .AddType<PublishedElementGraphType>()
                .AddInterfaceType<IPublishedPropertyGraphType>()
                .AddType<PublishedPropertyGraphType>();
            return requestExecutorBuilder;
        }

        public static IApplicationBuilder UseUHeadlessGraphQLEndpoint(this IApplicationBuilder applicationBuilder, string corsPolicy = null)
        {
            applicationBuilder.UseRouting();

            if (corsPolicy != null)
            {
                applicationBuilder.UseCors(corsPolicy);
            }
            else
            {
                applicationBuilder.UseCors();
            }

            applicationBuilder
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGraphQL();
                });
            return applicationBuilder;
        }

    }
}
