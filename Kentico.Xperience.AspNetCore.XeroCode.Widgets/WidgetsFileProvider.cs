using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Stores;

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets
{
    internal class WidgetsFileProvider : IFileProvider
    {
        private readonly IWidgetsStore widgetsStore;
        private readonly IWidgetInfoProvider widgetInfoProvider;

        private readonly Regex VirtualPathRegex = new Regex(@"\/Views\/Shared\/Widgets\/_(.*)\.cshtml");

        public IDictionary<string, IChangeToken> ChangeTokenDictionary = new Dictionary<string, IChangeToken>();

        public WidgetsFileProvider(
            IWidgetsStore widgetsStore,
            IWidgetInfoProvider widgetInfoProvider
            )
        {
            this.widgetsStore = widgetsStore;
            this.widgetInfoProvider = widgetInfoProvider;
        }

        public IDirectoryContents GetDirectoryContents(string subpath) => throw new NotImplementedException();

        public IFileInfo GetFileInfo(string subpath)
        {
            var identifier = VirtualPathRegex.Match(subpath).Groups[1];

            if (widgetsStore.Widgets.TryGetValue(identifier.Value, out var widget))
            {
                return new WidgetViewFileInfo(widget);
            }

            return new NotFoundFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            var identifier = VirtualPathRegex.Match(filter).Groups[1];

            if (widgetsStore.Widgets.TryGetValue(identifier.Value, out var widget))
            {
                if (ChangeTokenDictionary.TryGetValue(filter, out var changeToken))
                {
                    return changeToken;
                }
                else
                {
                    var newChangeToken = new WidgetChangeToken(widget, widgetInfoProvider);

                    ChangeTokenDictionary.Add(filter, newChangeToken);

                    return newChangeToken;
                }
            }

            return NullChangeToken.Singleton;
        }
    }
}