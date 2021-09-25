﻿using Nikcio.Umbraco.Headless.Core.Commands.Mappers.Pages;
using Nikcio.Umbraco.Headless.Core.Commands.PropertyMappers;
using Nikcio.Umbraco.Headless.Core.Mappers.SiteData;
using Nikcio.Umbraco.Headless.Core.Models.SiteData.Elements;
using Umbraco.Cms.Core.Models.PublishedContent;
using UmbracoConstants = Umbraco.Cms.Core.Constants;

namespace Nikcio.Umbraco.Headless.Core.Factories
{
    public class PageDataFactory : IPageDataFactory
    {
        public ICreatePropertyCommandBase CreatePropertyCommandBase { get; protected set; }

        public PageDataFactory(ICreatePropertyCommandBase createPropertyCommandBase)
        {
            AddPropertyMapDefaults();
            CreatePropertyCommandBase = createPropertyCommandBase;
        }

        private void AddPropertyMapDefaults()
        {
            if (!PropertyMapper.PropertyMap.ContainsKey(Constants.Constants.Factories.DefaultKey))
            {
                PropertyMapper.PropertyMap.Add(Constants.Constants.Factories.DefaultKey, (x) => new PropertyModel(x));
            }
            if (!PropertyMapper.PropertyMap.ContainsKey(UmbracoConstants.PropertyEditors.Aliases.BlockList))
            {
                PropertyMapper.PropertyMap.Add(UmbracoConstants.PropertyEditors.Aliases.BlockList, (x) => new BlockPropertyModel(x));
            }
        }

        public void SetCreatePropertyCommandBase(ICreatePageCommandBase createPageCommandBase)
        {
            CreatePropertyCommandBase.SetCreatePropertyCommandBase(createPageCommandBase);
        }

        public IPropertyModelBase GetPropertyData()
        {
            return PropertyMapper.PropertyMap.ContainsKey(CreatePropertyCommandBase.Property.PropertyType.EditorAlias)
                ? PropertyMapper.PropertyMap[CreatePropertyCommandBase.Property.PropertyType.EditorAlias].Invoke(CreatePropertyCommandBase)
                : PropertyMapper.PropertyMap[Constants.Constants.Factories.DefaultKey].Invoke(CreatePropertyCommandBase);
        }
    }
}
