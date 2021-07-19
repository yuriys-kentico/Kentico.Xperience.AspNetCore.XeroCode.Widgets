using Kentico.Xperience.AspNetCore.XeroCode.Resources.Stores;

using Microsoft.AspNetCore.Mvc;

namespace Kentico.Xperience.AspNetCore.XeroCode.Resources.Controllers
{
    public class ResourcesController : ControllerBase
    {
        private readonly IResourcesStore resourcesStore;

        public ResourcesController(IResourcesStore resourcesStore)
        {
            this.resourcesStore = resourcesStore;
        }

        [HttpGet]
        [ResponseCache(Duration = 60 * 60)]
        [Route("xerocode/resources/{*resourceName}")]
        public ActionResult Get(string resourceName)
        {
            var resource = resourcesStore.GetBytes(resourceName);

            if (resource != null)
            {
                return File(resource, GetMediaType(resourceName), resourceName);
            }

            return NotFound();
        }

        private string GetMediaType(string resourceName)
        {
            return resourceName switch
            {
                var _ when resourceName.EndsWith(".js") => "text/javascript",
                var _ when resourceName.EndsWith(".css") => "text/css",
                var _ when resourceName.EndsWith(".ttf") => "font/ttf",
                _ => "application/octet-stream",
            };
        }
    }
}