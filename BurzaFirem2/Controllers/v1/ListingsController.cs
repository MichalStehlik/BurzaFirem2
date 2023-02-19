using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BurzaFirem2.Data;
using BurzaFirem2.Models;
using Microsoft.AspNetCore.Authorization;
using BurzaFirem2.Constants;
using BurzaFirem2.ViewModels;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ListingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Listings
        [HttpGet]
        public async Task<ActionResult<ListVM<Listing>>> GetListing(
            string? name = null, 
            bool? visible = null,
            int page = 0,
            int pagesize = 0,
            string? order = null
            )
        {
            IQueryable<Listing> listings = _context.Listings;
            int total = listings.CountAsync().Result;
            if (!String.IsNullOrEmpty(name))
                listings = listings.Where(i => (i.Name.Contains(name)));
            if (visible != null)
            {
                listings = listings.Where(i => (i.Visible == visible));
            }
            int filtered = listings.CountAsync().Result;
            listings = order switch
            {
                "name" => listings.OrderBy(c => c.Name),
                "name_desc" => listings.OrderByDescending(c => c.Name),
                _ => listings.OrderByDescending(c => c.Created)
            };
            if (pagesize != 0)
            {
                listings = listings.Skip(page * pagesize).Take(pagesize);
            }
            int count = listings.CountAsync().Result;
            return new ListVM<Listing> { Total = total, Filtered = filtered, Count = count, Page = page, Pagesize = pagesize, Data = await listings.ToListAsync() };
        }

        // GET: api/Listings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Listing>> GetListing(int id)
        {
            var listing = await _context.Listing.FindAsync(id);

            if (listing == null)
            {
                return NotFound();
            }

            return listing;
        }

        // PUT: api/Listings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult> PutListing(int id, Listing listing)
        {
            if (id != listing.ListingId)
            {
                return BadRequest();
            }

            _context.Entry(listing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListingExists(id))
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

        // POST: api/Listings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<ActionResult<Listing>> PostListing(Listing listing)
        {
            _context.Listing.Add(listing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetListing", new { id = listing.ListingId }, listing);
        }

        // DELETE: api/Listings/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Security.ADMIN_POLICY)]
        public async Task<IActionResult> DeleteListing(int id)
        {
            var listing = await _context.Listing.FindAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            _context.Listing.Remove(listing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListingExists(int id)
        {
            return _context.Listing.Any(e => e.ListingId == id);
        }
    }
}
