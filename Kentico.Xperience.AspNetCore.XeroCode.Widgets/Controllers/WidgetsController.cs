using System;
using System.Linq;
using System.Text.Json;

using CMS.Helpers;

using Kentico.Forms.Web.Mvc;
using Kentico.Xperience.AspNetCore.XeroCode.Resources.Stores;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Models;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Stores;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Controllers
{
    [ApiController]
    [Authorize(Policy = nameof(VirtualContext))]
    public class WidgetsController : Controller
    {
        private readonly IWidgetsStore widgetsStore;
        private readonly IWidgetInfoProvider widgetInfoProvider;
        private readonly IWidgetsService widgetsService;
        private readonly IResourcesStore resourcesStore;

        public WidgetsController(
            IWidgetsStore widgetsStore,
            IWidgetInfoProvider widgetInfoProvider,
            IWidgetsService widgetsService,
            IResourcesStore resourcesStore
            )
        {
            this.widgetsStore = widgetsStore;
            this.widgetInfoProvider = widgetInfoProvider;
            this.widgetsService = widgetsService;
            this.resourcesStore = resourcesStore;
        }

        [HttpGet]
        [Route("xerocode/widgets")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("xerocode/widgets/all")]
        public IActionResult All()
        {
            return Json(new
            {
                widgets = widgetsStore.Widgets
                    .Values
                    .OrderBy(widget => widget.Id)
            });
        }

        [HttpPost]
        [Route("xerocode/widgets/upsert")]
        public IActionResult Upsert(Widget widget)
        {
            foreach (var property in widget.Properties)
            {
                property.TypeName = GetTypeForComponent(property.FormComponentIdentifier);
                property.DefaultValue = GetDefaultValue(property);
            }

            widgetInfoProvider.Set(new WidgetInfo
            {
                WidgetID = widget.Id,
                WidgetGuid = widget.Guid,
                WidgetName = widget.Name,
                WidgetDescription = widget.Description,
                WidgetIcon = widget.Icon,
                WidgetProperties = JsonConvert.SerializeObject(widget.Properties),
                WidgetView = widget.View
            });

            var identifier = widget.Guid.ToString();

            widgetsService.Remove(identifier);
            widgetsService.Add(identifier);

            return Ok();
        }

        [HttpPost]
        [Route("xerocode/widgets/new/widget")]
        public IActionResult NewWidget()
        {
            var resources = resourcesStore.GetStrings(typeof(Localization.Resources), "en-us");

            Upsert(new Widget
            {
                Name = $"{resources["xerocode.widgets.newname"]} {DateTime.Now:G}",
                Icon = "icon-cogwheel-square",
                Guid = Guid.NewGuid()
            });

            return Json(new
            {
                widget = widgetsStore.Widgets
                    .Values
                    .OrderBy(widget => widget.Id)
                    .Last()
            });
        }

        [HttpPost]
        [Route("xerocode/widgets/new/property")]
        public IActionResult NewProperty()
        {
            var resources = resourcesStore.GetStrings(typeof(Localization.Resources), "en-us");

            return Json(new
            {
                property = new WidgetProperty
                {
                    Id = Guid.NewGuid(),
                    FormComponentIdentifier = TextInputComponent.IDENTIFIER,
                    Name = resources["xerocode.widgets.property.newname"],
                    Label = resources["xerocode.widgets.property.newname"],
                    DefaultValue = "",
                    ExplanationText = "",
                    Tooltip = "",
                }
            });
        }

        [HttpPost]
        [Route("xerocode/widgets/remove")]
        public IActionResult Remove(Widget widget)
        {
            var widgetInfo = widgetInfoProvider.Get(widget.Id);

            widgetInfoProvider.Delete(widgetInfo);

            var identifier = widget.Guid.ToString();

            widgetsService.Remove(identifier);

            return Ok();
        }

        private string GetTypeForComponent(string formComponentIdentifier)
        {
            switch (formComponentIdentifier)
            {
                case TextInputComponent.IDENTIFIER:
                    return typeof(string).FullName!;

                case TextAreaComponent.IDENTIFIER:
                    return typeof(string).FullName!;

                case EmailInputComponent.IDENTIFIER:
                    return typeof(string).FullName!;

                case USPhoneComponent.IDENTIFIER:
                    return typeof(string).FullName!;

                case IntInputComponent.IDENTIFIER:
                    return typeof(int).FullName!;

                case CheckBoxComponent.IDENTIFIER:
                    return typeof(bool).FullName!;
            };

            throw new Exception($"Property form component identifier '{formComponentIdentifier}' does not have a corresponding property type.");
        }

        private object? GetDefaultValue(WidgetProperty property)
        {
            if (property.DefaultValue is JsonElement defaultValue)
            {
                switch (defaultValue.ValueKind, property.FormComponentIdentifier)
                {
                    case (JsonValueKind.String, TextInputComponent.IDENTIFIER):
                    case (JsonValueKind.String, TextAreaComponent.IDENTIFIER):
                    case (JsonValueKind.String, EmailInputComponent.IDENTIFIER):
                    case (JsonValueKind.String, USPhoneComponent.IDENTIFIER):
                        return defaultValue.GetString();

                    case (JsonValueKind.Number, IntInputComponent.IDENTIFIER):
                        return defaultValue.GetInt32();

                    case (JsonValueKind.String, CheckBoxComponent.IDENTIFIER):
                        return bool.Parse(defaultValue.GetString());

                    case (JsonValueKind.True, CheckBoxComponent.IDENTIFIER):
                    case (JsonValueKind.False, CheckBoxComponent.IDENTIFIER):
                        return defaultValue.GetBoolean();
                }
            }

            return null;
        }
    }
}