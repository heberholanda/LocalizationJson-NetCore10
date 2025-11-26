using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Localization.Languages
{
    /// <summary>
    /// Factory for creating JsonStringLocalizer instances
    /// Implements the ASP.NET Core factory pattern for localization
    /// </summary>
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;

        public JsonStringLocalizerFactory(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Creates a localizer based on the resource type
        /// </summary>
        public IStringLocalizer Create(Type resourceSource) =>
            new JsonStringLocalizer(_cache);

        /// <summary>
        /// Creates a localizer based on the base name and location
        /// </summary>
        public IStringLocalizer Create(string baseName, string location) =>
            new JsonStringLocalizer(_cache);
    }
}
