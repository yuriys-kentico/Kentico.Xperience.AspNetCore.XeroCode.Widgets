using System;
using System.ComponentModel.DataAnnotations;

using Kentico.Forms.Web.Mvc;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models
{
    public class WidgetProperty
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FormComponentIdentifier { get; set; } = TextInputComponent.IDENTIFIER;

        [Required]
        public string Name { get; set; } = string.Empty;

        public string TypeName { get; set; } = typeof(string).FullName!;

        public string Label { get; set; } = string.Empty;

        public object? DefaultValue { get; set; } = string.Empty;

        public string ExplanationText { get; set; } = string.Empty;

        public string Tooltip { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}