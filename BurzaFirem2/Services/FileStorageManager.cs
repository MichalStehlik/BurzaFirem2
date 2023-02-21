using BurzaFirem2.Data;
using BurzaFirem2.Models;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using System.IO;
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

        private string[] bitmapFormats = new string[] { "image/jpeg", "image/png", "image/webp"};

        public FileStorageManager(ILogger<FileStorageManager> logger, ApplicationDbContext context, IWebHostEnvironment environment, IOptions<FileStorageOptions> options, IHttpContextAccessor hca)
        {
            _logger = logger;
            _context = context;
            _env = environment;
            _options = options.Value;
            _hca = hca;
        }

        public async Task<StoredImage?> StoreTus(ITusFile file, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Debug, "Storing file ", file.Id);

            string? filename;
            string? contentType;
            MemoryStream ims = new MemoryStream();
            MemoryStream oms1 = new MemoryStream();
            using (Stream content = await file.GetContentAsync(cancellationToken))
            {
                Dictionary<string, tusdotnet.Models.Metadata> metadata = await file.GetMetadataAsync(cancellationToken);
                filename = metadata.FirstOrDefault(m => m.Key == "filename").Value.GetString(System.Text.Encoding.UTF8);
                contentType = metadata.FirstOrDefault(m => m.Key == "contentType").Value.GetString(System.Text.Encoding.UTF8);
                content.CopyTo(ims);
            };

            if (bitmapFormats.Contains(contentType))
            {
                var userId = Guid.Parse(_hca.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
                IImageFormat format;
                int imageWidth;
                int imageHeight;
                Guid id = Guid.NewGuid();
                using (Image image = Image.Load(ims.ToArray(), out format))
                {
                    int largestSize = Math.Max(image.Height, image.Width);
                    if (largestSize > _options.MaximumSize)
                    {
                        if (image.Width < image.Height)
                        {
                            image.Mutate(x => x.Resize(0, _options.MaximumSize));
                        }
                        else
                        {
                            image.Mutate(x => x.Resize(_options.MaximumSize, 0));
                        }
                    };
                    imageWidth = image.Width;
                    imageHeight = image.Height;
                    try
                    {
                        var fileStream = File.Create(Path.Combine(_env.ContentRootPath, _options.StoragePath, id.ToString()));
                        image.Save(fileStream,format);
                        fileStream.Close();

                        var imageRecord = new StoredImage
                        {
                            ImageId = id,
                            OriginalName = filename,
                            ContentType = contentType,
                            UploaderId = userId,
                            Width = imageWidth,
                            Height = imageHeight,
                        };
                        await _context.Images.AddAsync(imageRecord);
                        await _context.SaveChangesAsync();
                        return imageRecord;
                    }
                    catch(Exception ex)
                    {
                        throw new IOException(ex.Message);
                    }
                }
            }
            return null;
        }

        public async Task<StoredImage> Store(IFormFile uploadedFile)
        {
            string originalFilename = uploadedFile.FileName;
            string contentType = uploadedFile.ContentType;
            MemoryStream content = new MemoryStream();
            await uploadedFile.CopyToAsync(content);
            return await ProcessFile(originalFilename, contentType, content);
        }

        public List<string> GetFileNames()
        {
            var files = new List<string>();
            var fullNames = Directory.GetFiles(Path.Combine(_env.ContentRootPath, _options.StoragePath)).ToList();
            foreach (var fn in fullNames)
            {
                files.Add(Path.GetFileName(fn));
            }
            return files;
        }

        public bool FileExists(Guid id)
        {
            var fullName = Path.Combine(_env.ContentRootPath, _options.StoragePath, id.ToString());
            return System.IO.File.Exists(fullName);
        }

        public string? FileName(Guid id)
        {
            var fullName = Path.Combine(_env.ContentRootPath, _options.StoragePath, id.ToString());
            return File.Exists(fullName) ? fullName : null;
        }

        public async Task<bool> DeleteFileAsync(Guid id)
        {
            var fullName = Path.Combine(_env.ContentRootPath, _options.StoragePath, id.ToString());
            var storedImage = await _context.Images.FindAsync(id);
            if (storedImage != null && System.IO.File.Exists(fullName))
            {
                try
                {
                    _context.Images.Remove(storedImage);
                    _context.SaveChanges();
                    File.Delete(fullName);
                    return true;
                }
                catch 
                {
                    return false;
                }
            }
            return false;          
        }

        private async Task<StoredImage> ProcessFile(string originalName, string contentType, MemoryStream contentStream)
        {
            var userId = Guid.Parse(_hca.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            IImageFormat format;
            int imageWidth;
            int imageHeight;
            Guid id = Guid.NewGuid();
            using (Image image = Image.Load(contentStream.ToArray(), out format))
            {
                int largestSize = Math.Max(image.Height, image.Width);
                if (image.Width < image.Height)
                {
                    image.Mutate(x => x.Resize(0, _options.MaximumSize));
                }
                else
                {
                    image.Mutate(x => x.Resize(_options.MaximumSize, 0));
                }
                imageWidth = image.Width;
                imageHeight = image.Height;
                var fs = File.Create(Path.Combine(_env.ContentRootPath, _options.StoragePath, id.ToString()));
                image.Save(fs, format);
                fs.Close();
                var imageRecord = new StoredImage
                {
                    ImageId = id,
                    OriginalName = originalName,
                    ContentType = contentType,
                    UploaderId = userId,
                    Width = imageWidth,
                    Height = imageHeight,
                };
                await _context.Images.AddAsync(imageRecord);
                await _context.SaveChangesAsync();
                return imageRecord;
            }
        }
    }

    public class FileStorageOptions
    {
        public string Endpoint { get; set; } = String.Empty;
        public string UploadPath { get; set; } = String.Empty;
        public string StoragePath { get; set; } = String.Empty;
        public int SquareSize { get; set; } = 128;
        public int MaximumSize { get; set; } = 1024;
    }
}