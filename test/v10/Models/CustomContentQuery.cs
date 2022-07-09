﻿using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.Extensions.Caching.Distributed;
using Nikcio.UHeadless.Basics.Content.Models;
using Nikcio.UHeadless.Basics.ContentTypes.Models;
using Nikcio.UHeadless.Basics.Properties.Models;
using Nikcio.UHeadless.Cache.Extensions;
using Nikcio.UHeadless.Cache.Redis.Attributes;
using Nikcio.UHeadless.Content.Enums;
using Nikcio.UHeadless.Content.Queries;
using Nikcio.UHeadless.Content.Repositories;
using Nikcio.UHeadless.Content.Router;
using Nikcio.UHeadless.Core.GraphQL.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace v10.Models
{
    [ExtendObjectType(typeof(Query))]
    public class CustomContentQuery : ContentQuery<BasicContent<BasicProperty, BasicContentType>, BasicProperty, BasicContentRedirect>
    {
        private readonly IDistributedCache _cache;

        public CustomContentQuery(IDistributedCache cache)
        {
            _cache = cache;
        }

        [Authorize]
        //[UseCacheing]
        [GraphQLDescription("Gets all the content items at root level.")]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public new async Task<IEnumerable<BasicContent<BasicProperty, BasicContentType>?>> GetContentAtRoot([Service(ServiceKind.Default)] IContentRepository<BasicContent<BasicProperty, BasicContentType>, BasicProperty> contentRepository, [GraphQLDescription("The culture.")] string? culture = null, [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false)
        {
            var response = base.GetContentAtRoot(contentRepository, culture, preview);

            return response;
        }

        [Authorize]
        public override Task<BasicContent<BasicProperty, BasicContentType>?> GetContentByAbsoluteRoute([Service(ServiceKind.Default)] IContentRouter<BasicContent<BasicProperty, BasicContentType>, BasicProperty, BasicContentRedirect> contentRouter, [GraphQLDescription("The route to fetch. Example '/da/frontpage/'.")] string route, [GraphQLDescription("The base url for the request. Example: 'https://localhost:4000'. Default is the current domain")] string baseUrl = "", [GraphQLDescription("The culture.")] string? culture = null, [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false, [GraphQLDescription("Modes for requesting by route")] RouteMode routeMode = RouteMode.Routing)
        {
            return base.GetContentByAbsoluteRoute(contentRouter, route, baseUrl, culture, preview, routeMode);
        }

        [Authorize]
        public override IEnumerable<BasicContent<BasicProperty, BasicContentType>?> GetContentByContentType([Service(ServiceKind.Default)] IContentRepository<BasicContent<BasicProperty, BasicContentType>, BasicProperty> contentRepository, [GraphQLDescription("The contentType to fetch.")] string contentType, [GraphQLDescription("The culture.")] string? culture = null)
        {
            return base.GetContentByContentType(contentRepository, contentType, culture);
        }

        [Authorize]
        public override BasicContent<BasicProperty, BasicContentType>? GetContentByGuid([Service(ServiceKind.Default)] IContentRepository<BasicContent<BasicProperty, BasicContentType>, BasicProperty> contentRepository, [GraphQLDescription("The id to fetch.")] Guid id, [GraphQLDescription("The culture to fetch.")] string? culture = null, [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false)
        {
            return base.GetContentByGuid(contentRepository, id, culture, preview);
        }

        [Authorize]
        public override BasicContent<BasicProperty, BasicContentType>? GetContentById([Service(ServiceKind.Default)] IContentRepository<BasicContent<BasicProperty, BasicContentType>, BasicProperty> contentRepository, [GraphQLDescription("The id to fetch.")] int id, [GraphQLDescription("The culture to fetch.")] string? culture = null, [GraphQLDescription("Fetch preview values. Preview will show unpublished items.")] bool preview = false)
        {
            return base.GetContentById(contentRepository, id, culture, preview);
        }
    }
}
