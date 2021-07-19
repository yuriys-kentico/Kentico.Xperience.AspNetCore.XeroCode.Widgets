using System;
using System.Collections.Generic;

namespace Kentico.Xperience.AspNetCore.XeroCode.Resources.Stores
{
    public interface IResourcesStore
    {
        string Create(string content);

        byte[]? GetBytes(string resourceName);

        IDictionary<string, string> GetStrings(Type resourceType, string cultureCode);
    }
}