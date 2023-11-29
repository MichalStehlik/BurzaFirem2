using BurzaFirem2.Constants;
using BurzaFirem2.Data;
using BurzaFirem2.InputModels;
using BurzaFirem2.Models;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ILogger<UsersController> _logger;
        private readonly UserManager<ApplicationUser> _um;
        private readonly RoleManager<IdentityRole<Guid>> _rm;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger, UserManager<ApplicationUser> um, RoleManager<IdentityRole<Guid>> rm)
        {
            _context = context;
            _logger = logger;
            _um = um;
            _rm = rm;
        }


        // GET: api/v1/<UsersController>
        [HttpGet]
        public async Task<ActionResult<ListVM<UserVM>>> GetUsers(
            string? search = null,
            string? email = null,
            string? username = null,
            int page = 0,
            int pagesize = 0,
            string? order = null
            )
        {
            IQueryable<ApplicationUser> users = _context.Users.AsQueryable();
            int total = users.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                users = users.Where(i => (i.UserName!.Contains(search)));
            if (!String.IsNullOrEmpty(email))
                users = users.Where(i => (i.Email!.Contains(email)));
            if (!String.IsNullOrEmpty(username))
                users = users.Where(i => (i.UserName!.Contains(username)));
            int filtered = users.CountAsync().Result;
            users = order switch
            {
                "email" => users.OrderBy(c => c.Email),
                "email_desc" => users.OrderByDescending(c => c.Email),
                "username" => users.OrderBy(c => c.UserName),
                "username_desc" => users.OrderByDescending(c => c.UserName),
                _ => users
            };
            if (pagesize != 0)
            {
                users = users.Skip(page * pagesize).Take(pagesize);
            }
            int count = users.CountAsync().Result;
            return new ListVM<UserVM> { 
                Total = total, 
                Filtered = filtered, 
                Count = count, 
                Page = page, 
                Pagesize = pagesize, 
                Data = await users.Select(u => new UserVM { 
                    Id = u.Id, 
                    UserName = u.UserName, 
                    Email = u.Email, 
                    EmailConfirmed = u.EmailConfirmed, 
                    Updated = u.Updated,
                    Created = u.Created
                }).ToListAsync() 
            };
        }

        // GET api/v1/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserVM>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(new UserVM { Id = user.Id, UserName = user.UserName, Email = user.Email, EmailConfirmed = user.EmailConfirmed });
        }

        // POST api/v1/<UsersController>
        [HttpPost]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult>CreateUser([FromBody] CreateUserIM values)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = values.UserName,
                Email = values.Email,
                EmailConfirmed = true,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };
            var result = await _um.CreateAsync(user, values.Password);
            if (result.Succeeded)
            {
                var currentUser = await _um.FindByNameAsync(user.UserName);
                if (values.Admin == true)
                {
                    var roleResult = await _um.AddToRoleAsync(currentUser!, "Administrátor");
                }
                if (values.Editor == true)
                {
                    var roleResult = await _um.AddToRoleAsync(currentUser!, "Editor");
                }
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            return BadRequest();
        }

        // PUT api/v1/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] EditUserIM input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }
            var user = await _um.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.Email = input.Email;
            user.UserName = input.UserName;
            await _um.UpdateAsync(user);

            return Ok();
        }

        // DELETE api/v1/<UsersController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _um.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var result = await _um.DeleteAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest();
        }

        // DELETE api/v1/<UsersController>/5/password
        [HttpDelete("{id}/password")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult> DeletePasswordAsync(Guid id)
        {
            var user = await _um.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            await _um.RemovePasswordAsync(user);

            return Ok();
        }

        // PUT api/v1/<UsersController>/5/password
        [HttpPut("{id}/password")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult> PutPasswordAsync(Guid id, [FromBody] PasswordUserIM input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            var user = await _um.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            await _um.RemovePasswordAsync(user);
            await _um.AddPasswordAsync(user,input.Password);

            return Ok();
        }

        // GET api/v1/<UsersController>/roles
        [HttpGet("roles")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<ActionResult<List<IdentityRole>>> GetRolesAsync()
        {
            var roles = await _rm.Roles.ToListAsync();
            return Ok(roles);
        }

        // GET api/v1/<UsersController>/5/roles
        [HttpGet("{id}/roles")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<ActionResult<List<IdentityRole>>> GetRolesAsync(Guid id)
        {
            var user = await _um.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _um.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}
