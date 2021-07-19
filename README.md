# Xero Code Widgets

## Database-only widgets for Kentico Xperience 13.

| [![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico)                   |                                    |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | :--------------------------------: |
| [![NuGet](https://img.shields.io/nuget/v/KenticoXperience.AspNetCore.XeroCode.Widgets.Admin.svg)](https://www.nuget.org/packages/KenticoXperience.AspNetCore.XeroCode.Widgets.Admin) |    NuGet package for the admin     |
| [![NuGet](https://img.shields.io/nuget/v/KenticoXperience.AspNetCore.XeroCode.Widgets.svg)](https://www.nuget.org/packages/KenticoXperience.AspNetCore.XeroCode.Widgets)             | NuGet package for your MVC project |
| [![NuGet](https://img.shields.io/nuget/v/KenticoXperience.AspNetCore.XeroCode.Resources.svg)](https://www.nuget.org/packages/KenticoXperience.AspNetCore.XeroCode.Resources)         |   NuGet package for localization   |

Xero Code Widgets is an extension for Kentico Xperience 13 that allows marketers to create page builder widgets without code or needing to restart the presentation application.

This repository contains the source code for the admin NuGet, the presentation NuGet, and a supporting NuGet for localization.

## Description

Xero Code Widgets is an extension for Kentico Xperience 13 that allows marketers to create page builder widgets without code or needing to restart the presentation application. The extension adds an admin application named _Xero code widgets_ where marketers can create, modify, and delete widgets defined in the database. The extension configures the MVC application to make these widgets available in the page builder.

Marketers can set the name, description, icon, properties, and view of widgets. Properties can use a subset of the [available form components ](https://docs.xperience.io/developing-websites/form-builder-development/reference-system-form-components) and can have a default value, a label, a tooltip, and an explanation. The view is any Razor syntax where the property values are available in `@Model.Properties.*` by property name.

## Requirements and prerequisites

- _Kentico Xperience 13_ installed.
  - Only the ASP.NET Core development model is supported.

## Installation

1. Use Visual Studio to open the solution with the admin project.
1. In the **NuGet Package Manager Console**, run `Install-Package KenticoXperience.AspNetCore.XeroCode.Widgets.Admin`.
1. Build and make a request to the admin where it is running.
1. Use Visual Studio to open the solution with your MVC project.
1. In the **NuGet Package Manager Console**, run `Install-Package KenticoXperience.AspNetCore.XeroCode.Widgets`.
1. In _Startup.cs_ locate the **ConfigureServices** method.
1. In that method, there should already be a call to `AddControllersWithViews()`. In the chain after this call, add a call to `AddXeroCodeWidgets()` (the extension method is in the`Kentico.Xperience.AspNetCore.XeroCode.Widgets.Extensions` namespace).
1. There should also already be a call to `services.AddKentico` with an arrow function with a body. If there is no arrow function with a body, define one. In the body, add a call to `services.UseXeroCodeWidgets()`.
1. Build and run in IIS Express, or publish the project where it is hosted.
1. In the admin, open **Xero code widgets**. You may need to set up permissions to see the application if you are not an administrator.
1. Use the UI to create widgets.
1. Open **Pages**.
1. Navigate to a page with the page builder feature and add any of the created widgets.
