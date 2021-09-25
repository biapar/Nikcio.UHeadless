﻿using Microsoft.Extensions.DependencyInjection;
using Nikcio.Umbraco.Headless.Core.Commands.Mappers.Pages;
using Nikcio.Umbraco.Headless.Core.Commands.PropertyMappers;
using Nikcio.Umbraco.Headless.Core.Commands.Sites;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Nikcio.Umbraco.Headless.Core.Commands
{
    public class CommandComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddScoped<ICreatePropertyCommandBase, CreatePropertyCommandBase>();
            builder.Services.AddScoped<ICreatePageCommandBase, CreatePageCommandBase>();
            builder.Services.AddScoped<ICreateSiteCommandBase, CreateSiteCommandBase>();
        }
    }
}
