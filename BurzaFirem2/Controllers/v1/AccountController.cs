using BurzaFirem2.Data;
using BurzaFirem2.Emails.ViewModels;
using BurzaFirem2.InputModels;
using BurzaFirem2.Models;
using BurzaFirem2.Services;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ILogger<AccountController> _logger;
        private ApplicationDbContext _context { get; set; }
        private readonly IEmailSender _mailer;
        private readonly RazorViewToStringRenderer _renderer;
        private readonly UserManager<ApplicationUser> _um;
        private readonly SignInManager<ApplicationUser> _sim;
        private readonly TokenService _ts;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext context, IEmailSender mailer, RazorViewToStringRenderer renderer, UserManager<ApplicationUser> um, SignInManager<ApplicationUser> sim, TokenService ts)
        {
            _logger = logger;
            _context = context;
            _mailer = mailer;
            _renderer = renderer;
            _um = um;
            _sim = sim;
            _ts = ts;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserVM>> GetUserAsync()
        {
            var userId = Guid.Parse(User!.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            var user = await _um.FindByIdAsync(userId.ToString());
            if (user is not null)
            {
                return new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed
                };
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> AuthenticatePasswordAsync(LoginIM credentials)
        {
            var result = await _sim.PasswordSignInAsync(credentials.UserName, credentials.Password, false, true);
            _logger.LogInformation("User " + credentials.UserName + " login resulted in " + result.ToString());
            if (result.Succeeded)
            {
                var user = await _um.FindByNameAsync(credentials.UserName);
                if (user is not null)
                {
                    var authenticationToken = await _ts.Issue(user);
                    return Ok(authenticationToken);
                }
                return NotFound();
            };
            if (result.IsLockedOut)
            {
                return BadRequest("user locked out");
            }
            if (result.IsNotAllowed) 
            {
                return BadRequest("login not allowed");
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterIM values)
        {
            ApplicationUser user = new ApplicationUser 
            { 
                UserName = values.Email, 
                Email = values.Email,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };
            var result = await _um.CreateAsync(user, values.Password);
            if (result.Succeeded)
            {
                var code = await _um.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                string appUrl = HtmlEncoder.Default.Encode(Request.Scheme + "://" + Request.Host.Value);
                string htmlBody = await _renderer.RenderViewToStringAsync("/Emails/Pages/ConfirmAccount.cshtml",
                new ConfirmEmailVM
                {
                    ConfirmationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code)),
                    User = user,
                    ConfirmEmailUrl = appUrl + "/account/email-confirmation?code=" + code + "&id=" + user.Id,
                    AppUrl = appUrl
                });

                await _mailer.SendEmailAsync(user.Email, "Potvrzení registrace", htmlBody);
                return Created(nameof(GetUserAsync), user);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("email-confirmation")]
        public async Task<IActionResult> ConfirmEmailAsync(EmailConfirmationIM input)
        {
            var user = await _um.FindByIdAsync(input.Id);
            if (user is not null)
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
                var result = await _um.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Email for " + user.Email + " was confirmed.");
                    return Ok();
                }
                _logger.LogError("Email for " + user.Email + " was not confirmed.");
                return BadRequest();
            }
            _logger.LogError("User " + input.Id + " does not exists.");
            return NotFound();
        }
    }
}
