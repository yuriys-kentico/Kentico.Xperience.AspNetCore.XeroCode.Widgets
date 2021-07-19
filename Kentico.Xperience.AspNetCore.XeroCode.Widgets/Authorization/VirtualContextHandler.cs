using CMS.Helpers;

using Microsoft.AspNetCore.Authorization;

using System;
using System.Threading.Tasks;

namespace Kentico.Xperience.AspNetCore.XeroCode.Widgets.Authorization
{
    internal class VirtualContextHandler : AuthorizationHandler<VirtualContextRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, VirtualContextRequirement requirement)
        {
            if (!VirtualContext.IsInitialized)
            {
                throw new InvalidOperationException("Virtual context is not initialized.");
            }

            context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}