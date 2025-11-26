using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Localization.Controllers
{
    /// <summary>
    /// Controller responsible for demonstrating localization (i18n) usage
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LocalizationController : ControllerBase
    {
        private readonly ILogger<LocalizationController> _logger;
        private readonly IStringLocalizer<LocalizationController> _localizer;

        public LocalizationController(ILogger<LocalizationController> logger, IStringLocalizer<LocalizationController> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        /// Returns a simple localized greeting
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var message = _localizer["hi"].ToString();
            return Ok(message);
        }

        /// <summary>
        /// Returns a personalized welcome message with the provided name
        /// </summary>
        /// <param name="name">Name of the person to greet</param>
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var message = string.Format(_localizer["welcome"], name);
            return Ok(message);
        }

        /// <summary>
        /// Returns all localization strings available for the current language
        /// </summary>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var message = _localizer.GetAllStrings();
            return Ok(message);
        }
    }
}
