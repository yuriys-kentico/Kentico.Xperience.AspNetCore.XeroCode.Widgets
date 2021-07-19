using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models
{
    public class Widget
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid Guid { get; set; }

        public DateTime? LastModified { get; set; }

        [JsonIgnore]
        public Type? Type { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Icon { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = true)]
        public string View { get; set; } = string.Empty;

        [Required]
        public IList<WidgetProperty> Properties { get; set; } = new List<WidgetProperty>();
    }
}