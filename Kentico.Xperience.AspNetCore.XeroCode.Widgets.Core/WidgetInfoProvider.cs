using CMS.DataEngine;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core
{
    [ProviderInterface(typeof(IWidgetInfoProvider))]
    public class WidgetInfoProvider : AbstractInfoProvider<WidgetInfo, WidgetInfoProvider>, IWidgetInfoProvider
    {
        public WidgetInfoProvider() : base(WidgetInfo.TYPEINFO)
        {
        }
    }
}