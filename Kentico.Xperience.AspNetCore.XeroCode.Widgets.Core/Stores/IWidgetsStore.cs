using System.Collections.Generic;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Stores
{
    public interface IWidgetsStore
    {
        IDictionary<string, Widget> Widgets { get; }
    }
}