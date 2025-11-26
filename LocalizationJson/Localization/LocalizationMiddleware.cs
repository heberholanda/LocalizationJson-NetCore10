using System.Globalization;

namespace Localization.Languages
{
    /// <summary>
    /// Middleware responsible for configuring culture based on the Accept-Language header from the request
    /// </summary>
    public class LocalizationMiddleware : IMiddleware
    {
        /// <summary>
        /// Processes the request and configures the culture for the current thread
        /// </summary>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Gets the language from the Accept-Language header
            var cultureKey = context.Request.Headers["Accept-Language"];
            if (!string.IsNullOrEmpty(cultureKey))
            {
                // Validates if the culture exists in the system
                if (DoesCultureExist(cultureKey))
                {
                    var culture = new CultureInfo(cultureKey);
                    // Sets the culture for the current thread (affects date, number formatting, etc.)
                    Thread.CurrentThread.CurrentCulture = culture;
                    // Sets the UI culture (affects resource strings)
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
            }
            await next(context);
        }

        /// <summary>
        /// Checks if a culture exists in the system
        /// </summary>
        /// <param name="cultureName">Culture name (e.g., pt-BR, en-US)</param>
        /// <returns>True if the culture exists, False otherwise</returns>
        private static bool DoesCultureExist(string cultureName)
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Any(culture => string.Equals(culture.Name, cultureName,
              StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
