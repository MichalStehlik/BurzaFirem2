using BurzaFirem2.Data;
using BurzaFirem2.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using tusdotnet.Interfaces;

namespace BurzaFirem2.Services
{
    public class FileStorageManager
    {
        private readonly ILogger<FileStorageManager> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly FileStorageOptions _options;
        private readonly IHttpContextAccessor _hca;

        public FileStorageManager(ILogger<FileStorageManager> logger, ApplicationDbContext context, IWebHostEnvironment environment, IOptions<FileStorageOptions> options, IHttpContextAccessor hca)
        {
            _logger = logger;
            _context = context;
            _env = environment;
            _options = options.Value;
            _hca = hca;
        }

        public async Task StoreTus(ITusFile file, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Debug, "Storing file ", file.Id);
            using Stream content = await file.GetContentAsync(cancellationToken);
            Dictionary<string, tusdotnet.Models.Metadata> metadata = await file.GetMetadataAsync(cancellationToken);
            string? filename = metadata.FirstOrDefault(m => m.Key == "filename").Value.GetString(System.Text.Encoding.UTF8);
            string? contentType = metadata.FirstOrDefault(m => m.Key == "contentType").Value.GetString(System.Text.Encoding.UTF8);
            bool isPublic = false;
            if (metadata.ContainsKey("ispublic"))
            {
                isPublic = metadata.FirstOrDefault(m => m.Key == "ispublic")!.Value.GetString(System.Text.Encoding.UTF8) == "1" ? true : false;
            }         
            _logger.Log(LogLevel.Debug, "Successfuly stored file {0} of {1}", filename, contentType);

            if (contentType.StartsWith("image"))
            {
                var userId = Guid.Parse(_hca.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                var imageRecord = new StoredImage
                {
                    ImageId = file.Id,
                    OriginalName = filename,
                    ContentType = contentType,
                    UploaderId = userId
                };
                await _context.Images.AddAsync(imageRecord);
                await _context.SaveChangesAsync();
            }
        }

        public List<string> GetFileNames()
        {
            var files = new List<string>();
            var fullNames = Directory.GetFiles(Path.Combine(_env.ContentRootPath, _options.Path)).ToList();
            foreach (var fn in fullNames)
            {
                files.Add(Path.GetFileName(fn));
            }
            return files;
        }
    }

    public class FileStorageOptions
    {
        public string Endpoint { get; set; } = String.Empty;
        public string Path { get; set; } = String.Empty;
        public int SquareSize { get; set; } = 128;
        public int MaximumSize { get; set; } = 1024;
    }
}