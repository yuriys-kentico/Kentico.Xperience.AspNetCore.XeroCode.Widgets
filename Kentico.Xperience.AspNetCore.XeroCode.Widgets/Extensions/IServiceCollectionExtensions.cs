using CMS.Helpers;

using Kentico.Xperience.AspNetCore.XeroCode.Resources.Extensions;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Authorization;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Core.Stores;
using Kentico.Xperience.AspNetCore.XeroCode.Widgets.Stores;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IMvcBuilder AddXeroCodeWidgets(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.Services
                .AddSingleton<IWidgetPropertiesGenerator, WidgetPropertiesGenerator>()
                .AddSingleton<IWidgetsStore, WidgetsStore>()
                .AddSingleton<IWidgetsService, WidgetsService>()
                .AddSingleton<IAuthorizationHandler, VirtualContextHandler>();

            mvcBuilder.Services
                .AddAuthorization(options =>
                    options.AddPolicy(
                        nameof(VirtualContext),
                        policy => policy.Requirements.Add(new VirtualContextRequirement())
                        )
                );

            mvcBuilder.Services
                .AddXeroCodeResources();

            var serviceProvider = mvcBuilder.Services.BuildServiceProvider();

            mvcBuilder
                .AddRazorRuntimeCompilation(configureOptions =>
                    configureOptions.FileProviders.Add(
                        new WidgetsFileProvider(
                            serviceProvider.GetRequiredService<IWidgetsStore>(),
                            serviceProvider.GetRequiredService<IWidgetInfoProvider>()
                            )
                        )
                    );

            return mvcBuilder;
        }

        public static IServiceCollection UseXeroCodeWidgets(this IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetRequiredService<IWidgetsService>().RegisterAll();

            return serviceCollection;
        }
    }
}