using System;
using System.Collections.Generic;
using System.Reflection;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Stores;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets
{
    internal class WidgetsService : IWidgetsService
    {
        private readonly IWidgetsStore widgetsStore;

        private Type? widgetComponentDefinitionStore;
        private MethodInfo? widgetComponentDefinitionStoreAddMethod;
        private object? widgetComponentDefinitionStoreInstance;
        private object? widgetComponentDefinitionStoreInstanceRegisteredDefinitions;
        private Dictionary<string, WidgetDefinition>? widgetComponentDefinitionStoreInstanceRegisteredDefinitionsRegister;

        private readonly ConstructorInfo widgetDefinitionConstructor = typeof(WidgetDefinition)
            .GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(Type) },
                null
            )
                ?? throw new Exception($"Could not load '{nameof(widgetDefinitionConstructor)}'.");

        private Type WidgetComponentDefinitionStore
        {
            get => widgetComponentDefinitionStore ??= (typeof(ComponentDefinition).Assembly
                .GetType("Kentico.Content.Web.Mvc.ComponentDefinitionStore`1")
                    ?? throw new Exception("Could not load 'Kentico.Content.Web.Mvc.ComponentDefinitionStore`1'."))
                .MakeGenericType(typeof(WidgetDefinition));
        }

        private object WidgetComponentDefinitionStoreInstance
        {
            get => widgetComponentDefinitionStoreInstance ??= (WidgetComponentDefinitionStore
                .GetProperty("Instance")
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStore)}.Instance'."))
                .GetValue(null)
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStore)}.Instance' value.");
        }

        private MethodInfo WidgetComponentDefinitionStoreAddMethod
        {
            get => widgetComponentDefinitionStoreAddMethod ??= WidgetComponentDefinitionStore
                .GetMethod("Add")
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStore)}.Add'.");
        }

        private Action<string, string?, string, string, string, Type?> WidgetComponentDefinitionStoreAdd
            => (string identifier, string? customViewName, string name, string description, string icon, Type? propertiesType)
            => WidgetComponentDefinitionStoreAddMethod
                    .Invoke(
                        WidgetComponentDefinitionStoreInstance,
                        new[] {
                            widgetDefinitionConstructor.Invoke(
                                new object?[] {
                                    identifier,
                                    customViewName,
                                    name,
                                    description,
                                    icon,
                                    propertiesType
                                }
                            )
                        }
                    );

        private object WidgetComponentDefinitionStoreInstanceRegisteredDefinitions
        {
            get => widgetComponentDefinitionStoreInstanceRegisteredDefinitions ??= ((WidgetComponentDefinitionStore
                .BaseType
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStore)}.BaseType'."))
                .GetField("registeredDefinitions", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStore)}.registeredDefinitions'."))
                .GetValue(WidgetComponentDefinitionStoreInstance)
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStore)}.registeredDefinitions' value.");
        }

        private Dictionary<string, WidgetDefinition> WidgetComponentDefinitionStoreInstanceRegisteredDefinitionsRegister
        {
            get => widgetComponentDefinitionStoreInstanceRegisteredDefinitionsRegister ??= (((WidgetComponentDefinitionStoreInstanceRegisteredDefinitions
                .GetType()
                .GetField("register", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStoreInstanceRegisteredDefinitions)}.register'."))
                .GetValue(widgetComponentDefinitionStoreInstanceRegisteredDefinitions)
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStoreInstanceRegisteredDefinitions)}.register' value."))
                as Dictionary<string, WidgetDefinition>
                    ?? throw new Exception($"Could not load '{nameof(WidgetComponentDefinitionStoreInstanceRegisteredDefinitionsRegister)}'."));
        }

        public WidgetsService(IWidgetsStore widgetsStore)
        {
            this.widgetsStore = widgetsStore;
        }

        public void RegisterAll()
        {
            foreach (var widgetEntry in widgetsStore.Widgets)
            {
                var widget = widgetEntry.Value;

                WidgetComponentDefinitionStoreAdd(
                    widgetEntry.Key,
                    null,
                    widget.Name,
                    widget.Description,
                    widget.Icon,
                    widget.Type
                    );
            }
        }

        public void Add(string identifier)
        {
            if (!widgetsStore.Widgets.TryGetValue(identifier, out var widget))
            {
                throw new Exception($"Identifier '{identifier}' not found in the store.");
            }

            WidgetComponentDefinitionStoreAdd(
                identifier,
                null,
                widget.Name,
                widget.Description,
                widget.Icon,
                widget.Type
                );
        }

        public void Remove(string identifier)
        {
            WidgetComponentDefinitionStoreInstanceRegisteredDefinitionsRegister.Remove(identifier);
        }
    }
}