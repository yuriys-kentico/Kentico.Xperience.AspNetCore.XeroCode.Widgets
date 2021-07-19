using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using CMS.Helpers;

using Kentico.Web.Mvc.Internal;
using Kentico.Xperience.AspNetCore.XeroCode.Resources.Core.Attributes;

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kentico.Xperience.AspNetCore.XeroCode.Resources.Stores
{
    public class ResourcesStore : IResourcesStore
    {
        private readonly CaseInsensitiveResourceManagerStringLocalizerFactory factory;

        public IDictionary<string, byte[]> Resources = GetResources();

        public ResourcesStore(ILoggerFactory loggerFactory)
        {
            factory = new CaseInsensitiveResourceManagerStringLocalizerFactory(Options.Create(new LocalizationOptions()), loggerFactory);
        }

        public string Create(string content)
        {
            var name = SecurityHelper.GetSHA2Hash(content);

            CacheHelper.Cache(() => content, new CacheSettings(60, name));

            return name;
        }

        public byte[]? GetBytes(string resourceName)
        {
            var resource = Resources
                    .FirstOrDefault(resource => resource.Key.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (resource.Value != null)
            {
                return resource.Value;
            }

            if (CacheHelper.TryGetItem(resourceName, out string value))
            {
                return Encoding.UTF8.GetBytes(value);
            }

            return null;
        }

        public IDictionary<string, string> GetStrings(Type resourceType, string cultureCode)
        {
            var localizer = factory.CreateLocalizerWithCulture(resourceType, cultureCode);
            var resources = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var localizedString in localizer.GetAllStrings())
            {
                resources.TryAdd(localizedString.Name, localizedString.Value);
            }

            return resources;
        }

        private static Dictionary<string, byte[]> GetResources()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var resources = new Dictionary<string, byte[]>();

            foreach (var assembly in assemblies)
            {
                if (assembly.GetCustomAttribute<RegisterResourcesAttribute>() != null)
                {
                    var manifestResourceNames = assembly.GetManifestResourceNames();

                    foreach (var manifestResourceName in manifestResourceNames)
                    {
                        var resourceStream = assembly.GetManifestResourceStream(manifestResourceName);

                        if (resourceStream != null)
                        {
                            using var stream = new MemoryStream();

                            resourceStream.CopyTo(stream);

                            resources.TryAdd(manifestResourceName, stream.ToArray());
                        }
                    }
                }
            }

            return resources;
        }
    }
}