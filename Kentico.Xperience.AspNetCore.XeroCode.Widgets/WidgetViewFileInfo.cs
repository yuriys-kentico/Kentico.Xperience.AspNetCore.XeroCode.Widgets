using System;
using System.IO;
using System.Text;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

using Microsoft.Extensions.FileProviders;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets
{
    internal class WidgetViewFileInfo : IFileInfo
    {
        private readonly string view;

        public bool Exists => true;

        public bool IsDirectory => false;

        public string? PhysicalPath => null;

        public string Name { get; }

        public long Length { get; }

        public DateTimeOffset LastModified { get; }

        public WidgetViewFileInfo(Widget widget)
        {
            view = widget.View;

            Name = widget.Name;
            Length = view.Length;
            LastModified = widget.LastModified ?? DateTimeOffset.MinValue;
        }

        public Stream CreateReadStream() => new MemoryStream(Encoding.UTF8.GetBytes(view));
    }
}