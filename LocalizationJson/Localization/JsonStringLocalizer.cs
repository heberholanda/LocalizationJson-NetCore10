using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Text;

namespace Localization.Languages
{
    /// <summary>
    /// Custom implementation of IStringLocalizer that loads strings from JSON files
    /// Uses distributed cache to improve performance
    /// </summary>
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly IDistributedCache _cache;
        private readonly string _basePath = "Localization/Languages";
        private readonly string filePath;

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
            // Sets the JSON file path based on the current thread culture
            filePath = $"{_basePath}/{Thread.CurrentThread.CurrentCulture.Name}.json";
        }

        /// <summary>
        /// Gets a localized string by key
        /// </summary>
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, value == null);
            }
        }

        /// <summary>
        /// Gets a formatted localized string with arguments
        /// </summary>
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
                    : actualValue;
            }
        }

        /// <summary>
        /// Returns all localized strings from the current JSON file
        /// </summary>
        /// <param name="includeParentCultures">Indicates whether to include parent cultures (not implemented)</param>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var languageValues = GetAllValuesFromJson(filePath);
            var localizedValues = new List<LocalizedString>();
            foreach (var language in languageValues)
                localizedValues.Add(new LocalizedString(language.Key, language.Value));

            return localizedValues;
        }

        /// <summary>
        /// Gets a string from the JSON file, using cache when available
        /// </summary>
        private string? GetString(string key)
        {
            if (File.Exists(Path.GetFullPath(filePath)))
            {
                // Tries to retrieve from cache first
                var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
                var cacheValue = _cache.GetString(cacheKey);
                if (!string.IsNullOrEmpty(cacheValue))
                {
                    return cacheValue;
                }

                // If not in cache, retrieves from JSON file
                var result = GetValueFromJSON(key, Path.GetFullPath(filePath));

                // Stores in cache if found
                if (!string.IsNullOrEmpty(result))
                {
                    _cache.SetString(cacheKey, result);
                }
                return result;
            }
            return default;
        }

        /// <summary>
        /// Searches for a specific value in the JSON file by key
        /// </summary>
        private string? GetValueFromJSON(string propertyName, string filePath)
        {
            if ((propertyName == null) || (filePath == null))
                return default;

            var languageValues = GetAllValuesFromJson(filePath);
            var localizedValues = new List<LocalizedString>();
            foreach (var language in languageValues)
            {
                if (language.Key == propertyName)
                {
                    return language.Value;
                }
            }

            return default;
        }

        /// <summary>
        /// Loads and deserializes all values from the language JSON file
        /// </summary>
        private Dictionary<string, string> GetAllValuesFromJson(string filePath) => 
            JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filePath));
    }
}
