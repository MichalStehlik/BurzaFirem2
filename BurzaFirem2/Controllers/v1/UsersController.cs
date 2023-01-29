using BurzaFirem2.Constants;
using BurzaFirem2.Data;
using BurzaFirem2.InputModels;
using BurzaFirem2.Models;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger, UserManager<ApplicationUser> um)
        {
            _context = context;
            _logger = logger;
            _um = um;
        }

        // GET: api/v1/<UsersController>
        [HttpGet]
        public async Task<ActionResult<ListVM<UserVM>>> GetUsers(
            string? search = null,
            string? email = null,
            int page = 0,
            int pagesize = 0,
            string? order = null
            )
        {
            IQueryable<ApplicationUser> users = _context.Users.AsQueryable();
            int total = users.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                users = users.Where(i => (i.UserName.Contains(search)));
            if (!String.IsNullOrEmpty(email))
                users = users.Where(i => (i.Email.Contains(email)));
            int filtered = users.CountAsync().Result;
            users = order switch
            {
                "email" => users.OrderBy(c => c.Email),
                "email_desc" => users.OrderByDescending(c => c.Email),
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
                Data = users.Select(u => new UserVM { Id = u.Id, UserName = u.UserName, Email = u.Email, EmailConfirmed = u.EmailConfirmed }).ToList() 
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

        // POST api/<UsersController>
        [HttpPost]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult>CreateUser([FromBody] RegisterIM values)
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
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }
            return BadRequest();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
