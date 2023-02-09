using System.ComponentModel.DataAnnotations;

namespace BurzaFirem2.Models
{
    public class StoredImage
    {
        [Key]
        public Guid ImageId { get; set; }
        public ApplicationUser? Uploader { get; set; }
        [Required]
        public Guid UploaderId { get; set; }
        [Required]
        public string OriginalName { get; set; } = String.Empty;
        [Required]
        public string ContentType { get; set; } = String.Empty;
        [Required]
        public int Width { get; set; } = 0;
        [Required]
        public int Height { get; set; } = 0;
        public Company? Company { get; set; }
        public int? CompanyId { get; set; }
        public Company? CompanyLogo { get; set; }
        public int? CompanyLogoId { get; set; }
        public ICollection<Thumbnail> Thumbnails { get; set; }
    }
}
