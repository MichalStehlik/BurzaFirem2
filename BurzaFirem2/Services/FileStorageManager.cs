using BurzaFirem2.Data;
using tusdotnet.Interfaces;

namespace BurzaFirem2.Services
{
    public class FileStorageManager
    {
        private ILogger<FileStorageManager> _logger;
        private readonly ApplicationDbContext _context;

        public FileStorageManager(ILogger<FileStorageManager> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task StoreTus(ITusFile file, CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Debug, "Storing file ", file.Id);
            using Stream content = await file.GetContentAsync(cancellationToken);
            Dictionary<string, tusdotnet.Models.Metadata> metadata = await file.GetMetadataAsync(cancellationToken);
            string? filename = metadata.FirstOrDefault(m => m.Key == "filename").Value.GetString(System.Text.Encoding.UTF8);
            string? filetype = metadata.FirstOrDefault(m => m.Key == "filetype").Value.GetString(System.Text.Encoding.UTF8);
            bool isPublic = metadata.FirstOrDefault(m => m.Key == "ispublic")!.Value.GetString(System.Text.Encoding.UTF8) == "1" ? true : false;
            _logger.Log(LogLevel.Debug, "Successfuly stored file {0} of {1}", filename, filetype);
            if (filetype.StartsWith("image"))
            {

            }
        }
    }
}
