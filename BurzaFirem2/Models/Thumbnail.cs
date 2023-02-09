using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BurzaFirem2.Models
{
    public class Thumbnail
    {
        [ForeignKey("FileId")]
        public StoredImage Image { get; set; }
        [Key]
        public Guid ImageId { get; set; }
        [Key]
        public ThumbnailType Type { get; set; }
        public byte[] Blob { get; set; }
    }
    public enum ThumbnailType
    {
        Square,
        SameAspectRatio
    }
}
