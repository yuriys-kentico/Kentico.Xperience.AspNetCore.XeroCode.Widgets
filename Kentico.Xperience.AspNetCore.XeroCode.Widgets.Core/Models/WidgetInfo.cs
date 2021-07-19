using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;

using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;

[assembly: RegisterObjectType(typeof(WidgetInfo), WidgetInfo.OBJECT_TYPE)]

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models
{
    [Serializable]
    public class WidgetInfo : AbstractInfo<WidgetInfo, IWidgetInfoProvider>
    {
        public const string OBJECT_TYPE = "xerocode.widget";

        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(
            typeof(WidgetInfoProvider),
            OBJECT_TYPE,
            "XeroCode.Widget",
            nameof(WidgetID),
            nameof(WidgetLastModified),
            nameof(WidgetGuid),
            null,
            nameof(WidgetName),
            null, null, null, null)
        {
            ModuleName = "XeroCodeWidgets",
            TouchCacheDependencies = true,
        };

        [DatabaseField]
        public virtual int WidgetID
        {
            get => ValidationHelper.GetInteger(GetValue(nameof(WidgetID)), 0);
            set => SetValue(nameof(WidgetID), value);
        }

        [DatabaseField]
        public virtual string WidgetName
        {
            get => ValidationHelper.GetString(GetValue(nameof(WidgetName)), string.Empty);
            set => SetValue(nameof(WidgetName), value, string.Empty);
        }

        [DatabaseField]
        public virtual string WidgetDescription
        {
            get => ValidationHelper.GetString(GetValue(nameof(WidgetDescription)), string.Empty);
            set => SetValue(nameof(WidgetDescription), value, string.Empty);
        }

        [DatabaseField]
        public virtual string WidgetIcon
        {
            get => ValidationHelper.GetString(GetValue(nameof(WidgetIcon)), string.Empty);
            set => SetValue(nameof(WidgetIcon), value, "icon-app-widgets");
        }

        [DatabaseField]
        public virtual string WidgetProperties
        {
            get => ValidationHelper.GetString(GetValue(nameof(WidgetProperties)), string.Empty);
            set => SetValue(nameof(WidgetProperties), value, string.Empty);
        }

        [DatabaseField]
        public virtual string WidgetView
        {
            get => ValidationHelper.GetString(GetValue(nameof(WidgetView)), string.Empty);
            set => SetValue(nameof(WidgetView), value);
        }

        [DatabaseField]
        public virtual Guid WidgetGuid
        {
            get => ValidationHelper.GetGuid(GetValue(nameof(WidgetGuid)), Guid.Empty);
            set => SetValue(nameof(WidgetGuid), value);
        }

        [DatabaseField]
        public virtual DateTime WidgetLastModified
        {
            get => ValidationHelper.GetDateTime(GetValue(nameof(WidgetLastModified)), DateTimeHelper.ZERO_TIME);
            set => SetValue(nameof(WidgetLastModified), value);
        }

        protected override void DeleteObject() => Provider.Delete(this);

        protected override void SetObject() => Provider.Set(this);

        protected WidgetInfo(SerializationInfo info, StreamingContext context) : base(info, context, TYPEINFO)
        {
        }

        public WidgetInfo() : base(TYPEINFO)
        {
        }

        public WidgetInfo(DataRow dr) : base(TYPEINFO, dr)
        {
        }
    }
}