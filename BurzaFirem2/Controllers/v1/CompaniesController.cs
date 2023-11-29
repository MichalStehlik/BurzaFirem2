using BurzaFirem2.Constants;
using BurzaFirem2.Data;
using BurzaFirem2.InputModels;
using BurzaFirem2.Models;
using BurzaFirem2.Services;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using System.Collections.Generic;
using System.Security.Claims;
using Contact = BurzaFirem2.Models.Contact;
using Security = BurzaFirem2.Constants.Security;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private ILogger<CompaniesController> _logger;
        private readonly IAuthorizationService _authorizationService;
        private FileStorageManager _fsm;

        public CompaniesController(ApplicationDbContext context, ILogger<CompaniesController> logger, IAuthorizationService authorizationService, FileStorageManager fsm)
        {
            _context = context;
            _logger = logger;
            _authorizationService = authorizationService;
            _fsm = fsm;
        }

        // GET: api/v1/Companies
        [HttpGet]
        public async Task<ActionResult<ListVM<Company>>> GetCompanies(
            string? search,
            string? order,
            string? name,
            string? branches,
            string? activities,
            int? listing = null,
            int page = 0,
            int pagesize = 0
            )
        {
            IQueryable<Company> companies = _context.Companies
                .Include(c => c.Branches)
                .Include(c => c.Activities)
                .Include(c => c.Listings);
            int total = companies.CountAsync().Result;
            if (!String.IsNullOrEmpty(search))
                companies = companies.Where(i => (i.Name.Contains(search)));
            if (!String.IsNullOrEmpty(name))
                companies = companies.Where(i => (i.Name.Contains(name)));
            if (!String.IsNullOrEmpty(branches))
            {
                List<int> branchesList = branches!.Split(',').Select(Int32.Parse).ToList();
                companies = companies.Where(c => c.Branches.Any(b => branchesList.Contains(b.BranchId)));
            }
            if (!String.IsNullOrEmpty(activities))
            {
                List<int> activitiesList = activities!.Split(',').Select(Int32.Parse).ToList();
                companies = companies.Where(c => c.Activities.Any(a => activitiesList.Contains(a.ActivityId)));
            }
            if (listing != null)
            {
                companies = companies.Where(i => (i.Listings.Contains(new Listing { ListingId = (int)listing})));
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
            return new ListVM<Company> { Total = total, Filtered = filtered, Count = count, Page = page, Pagesize = pagesize, Data = await companies.ToListAsync() };
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
            _context.Entry(company).Collection(s => s.Listings).Load();
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
                UserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value),
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

        // PUT: api/v1/Companies/5
        [HttpPut("{id}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<IActionResult> PutCompany(int id, CompanyIM input)
        {
            if (id != input.CompanyId)
            {
                return BadRequest();
            }

            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).State = EntityState.Modified;
            company.Name = input.Name;
            company.Description = input.Description;
            company.Wanted = input.Wanted;
            company.Offer = input.Offer;
            company.AddressStreet = input.AddressStreet;
            company.Municipality = input.Municipality;
            company.Updated = DateTime.UtcNow;
            company.CompanyBranches = input.CompanyBranches;
            company.CompanyUrl = input.CompanyUrl;
            company.PresentationUrl = input.PresentationUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (company == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // CONTACTS
        #region Contacts
        // GET: api/Companies/1/contacts
        [HttpGet("{id}/contacts")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetCompanyContacts(int id)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            return await _context.Contacts.Where(c => c.CompanyId == id).ToListAsync();
        }

        [HttpGet("{id}/contacts/{contactId}")]
        public async Task<ActionResult<IEnumerable<Contact>>> GetCompanyContacts(int id, int contactId)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound("company");
            }

            var contact = await _context.Contacts.Where(c => c.ContactId == contactId).SingleOrDefaultAsync();
            if (contact == null)
            {
                return NotFound("contact");
            }

            return await _context.Contacts.Where(c => c.CompanyId == id).ToListAsync();
        }

        [HttpPost("{id}/contacts")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> PostCompanyContact(int id, ContactIM input)
        {
            if (id != input.CompanyId)
            {
                return BadRequest();
            }

            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            var contact = new Contact
            {
                Name = input.Name,
                Email = input.Email,
                Phone = input.Phone,
                CompanyId = company.CompanyId
            };

            company.Updated = DateTime.Now;

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        [HttpPut("{id}/contacts/{contactId}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<IActionResult> PutCompanyContact(int id, int contactId, ContactIM input)
        {
            if (id != input.ContactId)
            {
                return BadRequest();
            }

            var contact = await _context.Contacts.Where(c => c.ContactId == contactId).SingleOrDefaultAsync();
            if (contact == null)
            {
                return NotFound("contact");
            }

            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(contact).State = EntityState.Modified;
            contact.Name = input.Name;
            contact.Email = input.Email;
            contact.Phone = input.Phone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (company == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}/contacts/{contactId}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<IActionResult> DeleteCompanyContact(int id, int contactId)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            var contact = await _context.Contacts.Where(c => c.ContactId == contactId).SingleOrDefaultAsync();
            if (contact == null)
            {
                return NotFound("contact");
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        // BRANCHES
        #region Branches

        // GET: api/Companies/1/branches
        [HttpGet("{id}/branches")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetCompanyBranches(int id)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            return await _context.Branches.Include(b => b.Companies).Where(b => b.Companies!.Contains(new Company { CompanyId = id })).ToListAsync();
        }

        [HttpPost("{id}/branches")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> PostCompanyBranch(int id, IdIM input)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.Where(b => b.BranchId == input.Id).SingleOrDefaultAsync();
            if (branch == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).Collection(s => s.Branches).Load();

            if (!company.Branches.Contains(branch))
            {
                company.Branches.Add(branch);
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        [HttpDelete("{id}/branches/{branchId}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> DeleteCompanyBranch(int id, int branchId)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var branch = await _context.Branches.Where(b => b.BranchId == branchId).SingleOrDefaultAsync();
            if (branch == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).Collection(s => s.Branches).Load();

            if (company.Branches.Contains(branch))
            {
                company.Branches.Remove(branch);
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        #endregion

        // Activities
        #region Activities

        // GET: api/Companies/1/activities
        [HttpGet("{id}/activities")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetCompanyActivities(int id)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            return await _context.Activities.Include(b => b.Companies).Where(b => b.Companies!.Contains(new Company { CompanyId = id })).ToListAsync();
        }

        [HttpPost("{id}/activities")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> PostCompanyActivity(int id, IdIM input)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound("company");
            }

            var activity = await _context.Activities.Where(a => a.ActivityId == input.Id).SingleOrDefaultAsync();
            if (activity == null)
            {
                return NotFound("activity");
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).Collection(s => s.Activities).Load();

            if (!company.Activities.Contains(activity))
            {
                company.Activities.Add(activity);
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        [HttpDelete("{id}/activities/{activityId}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> DeleteCompanyActivity(int id, int activityId)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.Where(b => b.ActivityId == activityId).SingleOrDefaultAsync();
            if (activity == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).Collection(s => s.Activities).Load();

            if (company.Activities.Contains(activity))
            {
                company.Activities.Remove(activity);
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        #endregion
        // LISTINGS
        #region Listings
        // GET: api/Companies/1/listings
        [HttpGet("{id}/listings")]
        public async Task<ActionResult<IEnumerable<Listing>>> GetCompanyListings(int id)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            return await _context.Listings.Include(b => b.Companies).Where(b => b.Companies.Contains(new Company { CompanyId = id })).ToListAsync();
        }

        [HttpPost("{id}/listings")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> PostCompanyListing(int id, IdIM input)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.Where(l => l.ListingId == input.Id).SingleOrDefaultAsync();
            if (listing == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).Collection(s => s.Listings).Load();

            if (!company.Listings.Contains(listing))
            {
                company.Listings.Add(listing);
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        [HttpDelete("{id}/listings/{listingId}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> DeleteCompanyListing(int id, int listingId)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var listing = await _context.Listings.Where(l => l.ListingId == listingId).SingleOrDefaultAsync();
            if (listing == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            _context.Entry(company).Collection(s => s.Listings).Load();

            if (company.Listings.Contains(listing))
            {
                company.Listings.Remove(listing);
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }
        #endregion
        #region Logo
        [HttpGet("{id}/logo")]
        public async Task<ActionResult<StoredImage?>> GetCompanyLogo(int id)
        {
            var company = await _context.Companies.Include(c => c.Logo).Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            return company.Logo;
        }

        [HttpDelete("{id}/logo")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<Contact>> DeleteCompanyLogo(int id)
        {
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }

            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            company.LogoId = null;
            company.Updated = DateTime.Now;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.CompanyId }, company);
        }

        [HttpPost("{id}/logo")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<StoredImage>> PostCompanyLogo(int id)
        {
            Guid uid = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault()!.Value);
            var company = await _context.Companies.Where(c => c.CompanyId == id).SingleOrDefaultAsync();
            if (company == null)
            {
                return NotFound();
            }
            var isAdmin = await _authorizationService.AuthorizeAsync(User, Security.ADMIN_POLICY);
            if (!User.HasClaim(ClaimTypes.NameIdentifier, company.UserId.ToString()) && !isAdmin.Succeeded)
            {
                return Unauthorized("only owner or privileged user can edit a company record");
            }

            if (Request.Form.Files.Count == 1)
            {
                var file = Request.Form.Files[0];                
                var record = await _fsm.Store(file);
                company.LogoId = record.ImageId;
                company.Updated = DateTime.Now;
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetStoredImage", new { id = record.ImageId }, record);
            }
            return BadRequest("pick only one file");
        }
        #endregion
    }
}
