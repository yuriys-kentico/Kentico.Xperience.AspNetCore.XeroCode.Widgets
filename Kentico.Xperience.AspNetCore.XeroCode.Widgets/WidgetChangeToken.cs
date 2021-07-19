using System;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

using Microsoft.Extensions.Primitives;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets
{
    internal class WidgetChangeToken : IChangeToken
    {
        private readonly Widget widget;
        private readonly IWidgetInfoProvider widgetInfoProvider;
        private DateTime lastChecked;

        public WidgetChangeToken(
            Widget widget,
            IWidgetInfoProvider widgetInfoProvider
            )
        {
            this.widget = widget;
            this.widgetInfoProvider = widgetInfoProvider;

            lastChecked = DateTime.Now;
        }

        public bool HasChanged
        {
            get
            {
                if (widgetInfoProvider.Get(widget.Id).WidgetLastModified > lastChecked)
                {
                    lastChecked = DateTime.Now;

                    return true;
                }

                lastChecked = DateTime.Now;

                return false;
            }
        }

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => throw new NotImplementedException();
    }
}