using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BurzaFirem2.Data;
using BurzaFirem2.Models;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoredImage>>> GetImages()
        {
            return await _context.Images.ToListAsync();
        }

        // GET: api/v1/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoredImage>> GetStoredImage(Guid id)
        {
            var storedImage = await _context.Images.FindAsync(id);

            if (storedImage == null)
            {
                return NotFound();
            }

            return storedImage;
        }

        // PUT: api/v1/Images/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoredImage(Guid id, StoredImage storedImage)
        {
            if (id != storedImage.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(storedImage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoredImageExists(id))
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

        // POST: api/v1/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StoredImage>> PostStoredImage(StoredImage storedImage)
        {
            _context.Images.Add(storedImage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStoredImage", new { id = storedImage.ImageId }, storedImage);
        }

        // DELETE: api/v1/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoredImage(Guid id)
        {
            var storedImage = await _context.Images.FindAsync(id);
            if (storedImage == null)
            {
                return NotFound();
            }

            _context.Images.Remove(storedImage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoredImageExists(Guid id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
