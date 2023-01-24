using BurzaFirem2.Constants;
using BurzaFirem2.Data;
using BurzaFirem2.InputModels;
using BurzaFirem2.Models;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ILogger<CompaniesController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public CompaniesController(ApplicationDbContext context, ILogger<CompaniesController> logger, IAuthorizationService authorizationService)
        {
            _context = context;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        // GET: api/v1/Companies
        [HttpGet]
        public async Task<ActionResult<ListVM<Company>>> GetCompanies(
            string search,
            string order,
            string name,
            string branches,
            string activities,
            int page = 0,
            int pagesize = 0
            )
        {
            IQueryable<Company> companies = _context.Companies
                .Include(c => c.Branches);
            int total = companies.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                companies = companies.Where(i => (i.Name.Contains(search)));
            if (!String.IsNullOrEmpty(name))
                companies = companies.Where(i => (i.Name.Contains(name)));
            if (!String.IsNullOrEmpty(branches))
            {
                List<int> branchesList = branches?.Split(',').Select(Int32.Parse).ToList();
                companies = companies.Where(c => c.Branches.Any(b => branchesList.Contains(b.BranchId)));
            }
            if (!String.IsNullOrEmpty(activities))
            {
                List<int> activitiesList = activities?.Split(',').Select(Int32.Parse).ToList();
                companies = companies.Where(c => c.Activities.Any(a => activitiesList.Contains(a.ActivityId)));
            }
            int filtered = companies.CountAsync().Result;
            companies = order switch
            {
                "name" => companies.OrderBy(c => c.Name),
                "name_desc" => companies.OrderByDescending(c => c.Name),
                _ => companies
            };
            if (pagesize != 0)
            {
                companies = companies.Skip(page * pagesize).Take(pagesize);
            }
            int count = companies.CountAsync().Result;
            return new ListVM<Company> { Total = total, Filtered = filtered, Count = count, Page = page, Pagesize = pagesize, Data = companies.ToList() };
        }

        // GET: api/v1/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }
            _context.Entry(company).Collection(s => s.Branches).Load();
            _context.Entry(company).Collection(s => s.Activities).Load();
            _context.Entry(company).Collection(s => s.Contacts).Load();
            _context.Entry(company).Reference(c => c.Logo).Load();

            return company;
        }

        [HttpPost]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Company>> PostCompany(CompanyIM input)
        {
            var company = new Company
            {
                Name = input.Name,
                AddressStreet = input.AddressStreet,
                Municipality = input.Municipality,
                CompanyUrl = input.CompanyUrl,
                PresentationUrl = input.PresentationUrl,
                Description = input.Description,
                Wanted = input.Wanted,
                Offer = input.Offer,
                CompanyBranches = input.CompanyBranches,
                UserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value),
                Created = DateTime.Now,
                Updated = DateTime.Now
            };
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        // DELETE: api/v1/Companies/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can delete a company record");
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
