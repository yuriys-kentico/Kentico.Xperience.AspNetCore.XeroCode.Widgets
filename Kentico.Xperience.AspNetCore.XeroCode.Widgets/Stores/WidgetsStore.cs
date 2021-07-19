using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using CMS.Helpers;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Stores;

using Newtonsoft.Json;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Stores
{
    internal class WidgetsStore : IWidgetsStore
    {
        private readonly IWidgetPropertiesGenerator widgetPropertiesGenerator;
        private readonly IWidgetInfoProvider widgetInfoProvider;

        public WidgetsStore(
            IWidgetPropertiesGenerator widgetPropertiesGenerator,
            IWidgetInfoProvider widgetInfoProvider
            )
        {
            this.widgetPropertiesGenerator = widgetPropertiesGenerator;
            this.widgetInfoProvider = widgetInfoProvider;
        }

        public IDictionary<string, Widget> Widgets => CacheHelper.Cache(cacheSettings =>
            {
                IDictionary<string, Widget> dictionary = new ConcurrentDictionary<string, Widget>();

                var widgetInfos = widgetInfoProvider.Get();

                widgetPropertiesGenerator.ReloadDynamicAssembly();

                foreach (var widgetInfo in widgetInfos)
                {
                    var widgetProperties = JsonConvert.DeserializeObject<IList<WidgetProperty>>(widgetInfo.WidgetProperties);

                    foreach (var property in widgetProperties)
                    {
                        if (property.DefaultValue != null)
                        {
                            property.DefaultValue = GetDefaultValue(property.DefaultValue);
                        }
                    }

                    dictionary.Add(
                        widgetInfo.WidgetGuid.ToString(),
                        new Widget
                        {
                            Id = widgetInfo.WidgetID,
                            Guid = widgetInfo.WidgetGuid,
                            LastModified = widgetInfo.WidgetLastModified,
                            Type = widgetPropertiesGenerator.GetPropertiesType(
                                $"Widget{widgetInfo.WidgetGuid:N}Properties",
                                widgetProperties
                            ),
                            Name = widgetInfo.WidgetName,
                            Description = widgetInfo.WidgetDescription,
                            Icon = widgetInfo.WidgetIcon,
                            View = widgetInfo.WidgetView,
                            Properties = widgetProperties
                        }
                    );
                }

                if (cacheSettings.Cached)
                {
                    cacheSettings.CacheDependency = CacheHelper.GetCacheDependency(WidgetInfo.OBJECT_TYPE + "|all");
                }

                return dictionary;
            }, new CacheSettings(60 * 24, $"{typeof(WidgetsStore).FullName}|{nameof(Widgets)}"));

        private object? GetDefaultValue(object defaultValue)
        {
            var defaultValueType = defaultValue.GetType();

            return defaultValueType switch
            {
                var _ when defaultValueType == typeof(long) => Convert.ToInt32(defaultValue),
                _ => defaultValue,
            };
        }
    }
}