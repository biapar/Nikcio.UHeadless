﻿using Nikcio.UHeadless.Models.Dtos.Elements;
using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Nikcio.UHeadless.Models.Dtos.Content
{
    public interface IPublishedContentGraphType : IPublishedElementGraphType
    {
        IEnumerable<IPublishedContentGraphType> Children { get; set; }
        IEnumerable<IPublishedContentGraphType> ChildrenForAllCultures { get; set; }
        DateTime CreateDate { get; set; }
        int CreatorId { get; set; }
        IReadOnlyDictionary<string, PublishedCultureInfo> Cultures { get; set; }
        int Id { get; set; }
        PublishedItemType ItemType { get; set; }
        int Level { get; set; }
        string Name { get; set; }
        IPublishedContentGraphType Parent { get; set; }
        string Path { get; set; }
        int SortOrder { get; set; }
        int? TemplateId { get; set; }
        DateTime UpdateDate { get; set; }
        string UrlSegment { get; set; }
        int WriterId { get; set; }
    }
}