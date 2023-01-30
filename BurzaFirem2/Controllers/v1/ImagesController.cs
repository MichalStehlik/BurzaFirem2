using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BurzaFirem2.Data;
using BurzaFirem2.Models;
using BurzaFirem2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BurzaFirem2.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace BurzaFirem2.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _um;
        private ILogger<ImagesController> _logger;
        private IWebHostEnvironment _environment;

        public ImagesController(ApplicationDbContext context, UserManager<ApplicationUser> um, ILogger<ImagesController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _um = um;
            _logger = logger;
            _environment = environment;
        }

        // GET: api/v1/Images
        [HttpGet]
        public async Task<ListVM<StoredImage>> GetImages(
            string? name = null,
            string? uploaderId = null,
            int page = 0,
            int pagesize = 0,
            string? order = null
            )
        {
            IQueryable<StoredImage> images = _context.Images;
            int total = images.CountAsync().Result;
            if (!String.IsNullOrEmpty(name))
                images = images.Where(i => (i.OriginalName.Contains(name)));
            if (uploaderId != null)
            {
                images = images.Where(i => (i.UploaderId == Guid.Parse(uploaderId)));
            }
            int filtered = images.CountAsync().Result;
            images = order switch
            {
                "name" => images.OrderBy(c => c.OriginalName),
                "name_desc" => images.OrderByDescending(c => c.OriginalName),
                _ => images.OrderByDescending(c => c.ImageId)
            };
            if (pagesize != 0)
            {
                images = images.Skip(page * pagesize).Take(pagesize);
            }
            int count = images.CountAsync().Result;
            return new ListVM<StoredImage> { Total = total, Filtered = filtered, Count = count, Page = page, Pagesize = pagesize, Data = images.ToList() };
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

        // POST: api/v1/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<StoredImage>> PostStoredImage()
        {
            ApplicationUser user = await _um.GetUserAsync(User);
            if (user != null && Request.Form.Files.Count == 1)
            {
                var file = Request.Form.Files[0];
                var path = Path.Combine(_environment.ContentRootPath, "Uploads", file.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                };
                var entry = new StoredImage {
                    OriginalName= file.FileName,
                    ContentType= file.ContentType,
                    UploaderId = user.Id
                };
                _context.Images.Add(entry);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetStoredImage", new { id = entry.ImageId }, entry);
            }
            return NotFound("user not found");
        }

        // DELETE: api/v1/Images/5
        [HttpDelete("{id}")]
        [Authorize(Policy = Security.EDITOR_POLICY)]
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
