﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nikcio.UHeadless.UmbracoElements.Properties.Models;
using Nikcio.UHeadless.UmbracoMedia.Media.Factories;
using Nikcio.UHeadless.UmbracoMedia.Media.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Nikcio.UHeadless.UmbracoMedia.Media.Repositories {
    /// <inheritdoc/>
    public class MediaRepository<TMedia, TProperty> : IMediaRepository<TMedia, TProperty>
        where TMedia : IMedia<TProperty>
        where TProperty : IProperty {
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
        private readonly IMediaFactory<TMedia, TProperty> _mediaFactory;

        /// <inheritdoc/>
        public MediaRepository(IPublishedSnapshotAccessor publishedSnapshotAccessor, IUmbracoContextFactory umbracoContextFactory, IMediaFactory<TMedia, TProperty> mediaFactory) {
            umbracoContextFactory.EnsureUmbracoContext();
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
            _mediaFactory = mediaFactory;
        }

        /// <inheritdoc/>
        public virtual TMedia? GetMedia(Func<IPublishedMediaCache?, IPublishedContent?> fetch, string? culture) {
            if (_publishedSnapshotAccessor.TryGetPublishedSnapshot(out var publishedSnapshot)) {
                var media = fetch(publishedSnapshot?.Media);
                if (media != null && culture == null || media != null) {
                    return GetConvertedMedia(media, culture);
                }
            }

            return default;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<TMedia?> GetMediaList(Func<IPublishedMediaCache?, IEnumerable<IPublishedContent>?> fetch, string? culture) {
            if (_publishedSnapshotAccessor.TryGetPublishedSnapshot(out var publishedSnapshot)) {
                var MediaList = fetch(publishedSnapshot?.Media);
                if (MediaList != null) {
                    return MediaList.Select(Media => GetConvertedMedia(Media, culture));
                }
            }

            return new List<TMedia>();
        }

        /// <inheritdoc/>
        public virtual TMedia? GetConvertedMedia(IPublishedContent media, string? culture) {
            return _mediaFactory.CreateMedia(media, culture);
        }
    }
}