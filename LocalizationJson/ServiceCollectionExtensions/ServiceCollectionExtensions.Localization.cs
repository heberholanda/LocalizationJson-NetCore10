using Localization.Languages;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LocalizationJson.ServiceCollectionExtensions
{
    /// <summary>
    /// Extensions for configuring localization services
    /// </summary>
    public static class ServiceCollectionExtensionsLocalization
    {
        /// <summary>
        /// Adds the necessary services for JSON-based localization
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddLocalizationConfiguration(this IServiceCollection services)
        {
            // Adds ASP.NET Core base localization services
            services.AddLocalization();
            
            // Registers the custom localization middleware
            services.AddSingleton<LocalizationMiddleware>();
            
            // Adds distributed memory cache for performance
            services.AddDistributedMemoryCache();
            
            // Registers the custom factory that uses JSON files
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            return services;
        }

        /// <summary>
        /// Configures the application's localization pipeline
        /// </summary>
        /// <param name="app">Web application instance</param>
        /// <returns>The web application for chaining</returns>
        public static WebApplication UseLocalizationConfiguraton(this WebApplication app)
        {
            // Defines the cultures supported by the application
            var supportedCultures = new[] 
            { 
                new CultureInfo("pt-BR"), // Brazilian Portuguese
                new CultureInfo("en-US")  // United States English
            };
            
            // Configures localization options
            var options = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US")),
                SupportedCultures = supportedCultures
            };
            
            // Enables request localization
            app.UseRequestLocalization(options);
            
            // Adds the custom middleware that reads the Accept-Language header
            app.UseMiddleware<LocalizationMiddleware>();

            return app;
        }
    }
}
