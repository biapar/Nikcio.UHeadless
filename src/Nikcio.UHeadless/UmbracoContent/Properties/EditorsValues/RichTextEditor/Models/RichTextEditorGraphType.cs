﻿using HotChocolate;
using Nikcio.UHeadless.UmbracoContent.Properties.Bases.Models;
using Nikcio.UHeadless.UmbracoContent.Properties.EditorsValues.Default.Commands;
using Umbraco.Cms.Core.Strings;

namespace Nikcio.UHeadless.UmbracoContent.Properties.EditorsValues.RichTextEditor.Models
{
    [GraphQLDescription("Represents a rich text editor.")]
    public class RichTextEditorGraphType : PropertyValueBaseGraphType
    {
        [GraphQLDescription("Gets the value of the rich text editor.")]
        public string Value { get; set; }

        public RichTextEditorGraphType(CreatePropertyValue createPropertyValue) : base(createPropertyValue)
        {
            Value = ((IHtmlEncodedString)createPropertyValue.Property.GetValue(createPropertyValue.Culture)).ToHtmlString();
        }
    }
}