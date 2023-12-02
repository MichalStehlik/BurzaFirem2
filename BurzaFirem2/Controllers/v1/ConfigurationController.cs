using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private IConfiguration _configuration;
        private ILogger<ConfigurationController> _logger;

        public ConfigurationController(IConfiguration configuration, ILogger<ConfigurationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet("application")]
        public object GetApplicationConfiguration()
        {
            return new
            {
                ApplicationName = _configuration["Application:Name"],
                PasswordLength = Int32.Parse(_configuration["Password:Length"]!),
                PasswordUppercase = _configuration["Password:Uppercase"],
                PasswordLowercase = _configuration["Password:Lowercase"],
                PasswordDigit = _configuration["Password:Digit"],
                PasswordNonalphanumeric = _configuration["Password:NonAlphaNumeric"],
                Debug = _configuration["Application:Debug"] ?? "0",
            };
        }
    }
}
