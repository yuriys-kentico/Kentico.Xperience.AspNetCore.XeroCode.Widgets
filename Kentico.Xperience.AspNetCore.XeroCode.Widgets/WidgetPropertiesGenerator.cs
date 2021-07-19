using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets
{
    internal class WidgetPropertiesGenerator : IWidgetPropertiesGenerator
    {
        private ModuleBuilder module;

        public WidgetPropertiesGenerator()
        {
            module = DefineModule();
        }

        public void ReloadDynamicAssembly() => module = DefineModule();

        public Type GetPropertiesType(string identifier, IList<WidgetProperty> widgetProperties)
        {
            var dynamicType = module.DefineType(identifier, TypeAttributes.Public, null, new[] { typeof(IWidgetProperties) });

            foreach (var widgetProperty in widgetProperties)
            {
                var propertyName = widgetProperty.Name;
                var propertyType = Type.GetType(widgetProperty.TypeName, true, true)
                    ?? throw new Exception($"Could not load '{widgetProperty.TypeName}'.");

                var field = dynamicType.DefineField($"{propertyName}Field", propertyType, FieldAttributes.Private);

                var getSetAttributes = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                var property = dynamicType.DefineProperty(propertyName, PropertyAttributes.None, propertyType, null);

                var getProperty = dynamicType.DefineMethod($"get_{propertyName}", getSetAttributes, propertyType, Type.EmptyTypes);
                var getPropertyIlGenerator = getProperty.GetILGenerator();
                getPropertyIlGenerator.Emit(OpCodes.Ldarg_0);
                getPropertyIlGenerator.Emit(OpCodes.Ldfld, field);
                getPropertyIlGenerator.Emit(OpCodes.Ret);
                property.SetGetMethod(getProperty);

                var setProperty = dynamicType.DefineMethod($"set_{propertyName}", getSetAttributes, null, new Type[] { propertyType });
                var setPropertyIlGenerator = setProperty.GetILGenerator();
                setPropertyIlGenerator.Emit(OpCodes.Ldarg_0);
                setPropertyIlGenerator.Emit(OpCodes.Ldarg_1);
                setPropertyIlGenerator.Emit(OpCodes.Stfld, field);
                setPropertyIlGenerator.Emit(OpCodes.Ret);
                property.SetSetMethod(setProperty);

                var editingComponentAttributeConstructor = typeof(EditingComponentAttribute).GetConstructor(new Type[] { typeof(string) })
                    ?? throw new Exception($"Could not load '{nameof(EditingComponentAttribute)}(string)' constructor.");

                var editingComponentAttributeProperties = new Dictionary<PropertyInfo, object>
                {
                    {  typeof(EditingComponentAttribute).GetProperty(nameof(WidgetProperty.Label))
                        ?? throw new Exception($"Could not load '{nameof(EditingComponentAttribute)}.{nameof(WidgetProperty.Label)}'."), widgetProperty.Label },
                    {  typeof(EditingComponentAttribute).GetProperty(nameof(WidgetProperty.ExplanationText))
                        ?? throw new Exception($"Could not load '{nameof(EditingComponentAttribute)}.{nameof(WidgetProperty.ExplanationText)}'."), widgetProperty.ExplanationText },
                    {  typeof(EditingComponentAttribute).GetProperty(nameof(WidgetProperty.Tooltip))
                        ?? throw new Exception($"Could not load '{nameof(EditingComponentAttribute)}.{nameof(WidgetProperty.Tooltip)}'."), widgetProperty.Tooltip },
                    {  typeof(EditingComponentAttribute).GetProperty(nameof(WidgetProperty.Order))
                        ?? throw new Exception($"Could not load '{nameof(EditingComponentAttribute)}.{nameof(WidgetProperty.Order)}'."), widgetProperty.Order }
                };

                if (widgetProperty.DefaultValue != null)
                {
                    editingComponentAttributeProperties.Add(
                        typeof(EditingComponentAttribute).GetProperty(nameof(WidgetProperty.DefaultValue))
                            ?? throw new Exception($"Could not load '{nameof(EditingComponentAttribute)}.{nameof(WidgetProperty.DefaultValue)}'.")
                        , widgetProperty.DefaultValue);
                }

                property.SetCustomAttribute(
                    new CustomAttributeBuilder(
                        editingComponentAttributeConstructor,
                        new[] { widgetProperty.FormComponentIdentifier },
                        editingComponentAttributeProperties.Keys.ToArray(),
                        editingComponentAttributeProperties.Values.ToArray()
                    )
                );
            }

            var generatedType = dynamicType.CreateType();

            return generatedType
                ?? throw new Exception($"Could not create type '{identifier}'.");
        }

        private static ModuleBuilder DefineModule()
        {
            return AssemblyBuilder
                .DefineDynamicAssembly(new AssemblyName("XeroCodeWidgets"), AssemblyBuilderAccess.Run)
                .DefineDynamicModule("XeroCodeWidgetsModule" + Guid.NewGuid().ToString());
        }
    }
}