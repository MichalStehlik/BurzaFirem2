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
using BurzaFirem2.Services;

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
        private FileStorageManager _fsm;

        public ImagesController(ApplicationDbContext context, UserManager<ApplicationUser> um, ILogger<ImagesController> logger, IWebHostEnvironment environment, FileStorageManager fsm)
        {
            _context = context;
            _um = um;
            _logger = logger;
            _environment = environment;
            _fsm = fsm;
        }

        // GET: api/v1/Images
        [HttpGet]
        public async Task<ListVM<StoredImage>> GetImages(
            string? originalName = null,
            string? uploaderId = null,
            int page = 0,
            int pagesize = 0,
            string? order = null
            )
        {
            IQueryable<StoredImage> images = _context.Images.Include(i => i.Uploader);
            int total = images.CountAsync().Result;
            if (!String.IsNullOrEmpty(originalName))
                images = images.Where(i => (i.OriginalName.Contains(originalName)));
            if (uploaderId != null)
            {
                images = images.Where(i => (i.UploaderId == Guid.Parse(uploaderId)));
            }
            int filtered = images.CountAsync().Result;
            images = order switch
            {
                "originalName" => images.OrderBy(c => c.OriginalName),
                "originalName_desc" => images.OrderByDescending(c => c.OriginalName),
                _ => images.OrderByDescending(c => c.ImageId)
            };
            if (pagesize != 0)
            {
                images = images.Skip(page * pagesize).Take(pagesize);
            }
            int count = images.CountAsync().Result;
            return new ListVM<StoredImage> { Total = total, Filtered = filtered, Count = count, Page = page, Pagesize = pagesize, Data = await images.ToListAsync() };
        }

        [HttpGet("filenames")]
        public List<string> GetFilenames()
        {
            return _fsm.GetFileNames();
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

        [HttpGet("{id}/content")]
        public async Task<ActionResult> GetStoredImageContent(Guid id)
        {
            var storedImage = await _context.Images.FindAsync(id);

            if (storedImage == null)
            {
                return NotFound("no file record exists");
            }

            if (!_fsm.FileExists(id))
            {
                return NotFound("file does not physically exists");
            }
            return PhysicalFile(_fsm.FileName(id)!, storedImage.ContentType, storedImage.OriginalName);
        }

        // POST: api/v1/Images
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Policy = Security.EDITOR_POLICY)]
        public async Task<ActionResult<StoredImage>> PostStoredImage()
        {
            ApplicationUser? user = await _um.GetUserAsync(User);
            if (user != null && Request.Form.Files.Count == 1)
            {
                var file = Request.Form.Files[0];
                var record = await _fsm.Store(file);
                return CreatedAtAction("GetStoredImage", new { id = record.ImageId }, record);
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
            if (await _fsm.DeleteFileAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        private bool StoredImageExists(Guid id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
