using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILogger<AccountController> _logger;

    }
}
