using CMS.DataEngine;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core
{
    public interface IWidgetInfoProvider : IInfoProvider<WidgetInfo>, IInfoByIdProvider<WidgetInfo>, IInfoByNameProvider<WidgetInfo>
    {
    }
}