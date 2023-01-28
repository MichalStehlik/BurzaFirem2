using BurzaFirem2.Data;
using BurzaFirem2.Models;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ILogger<UsersController> _logger;

        public UsersController(ApplicationDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
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
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
