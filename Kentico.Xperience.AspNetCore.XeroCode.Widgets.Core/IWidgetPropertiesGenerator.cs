using System;
using System.Collections.Generic;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core
{
    public interface IWidgetPropertiesGenerator
    {
        void ReloadDynamicAssembly();

        Type GetPropertiesType(string identifier, IList<WidgetProperty> widgetProperties);
    }
}