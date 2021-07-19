using Kentico.Xperience.AspNetCore.XeroCode.Resources.Stores;

using Microsoft.Extensions.DependencyInjection;

namespace Kentico.Xperience.AspNetCore.XeroCode.Resources.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddXeroCodeResources(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IResourcesStore, ResourcesStore>();

            return serviceCollection;
        }
    }
}