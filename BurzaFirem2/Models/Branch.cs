using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

namespace BurzaFirem2.Models
{
    public class Branch
    {
        public int BranchId { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [NotMapped]
        public string BackgroundColor { get; set; } = "#ffffff";
        public string TextColor { get; set; } = "#000000";
        [JsonIgnore]
        public ICollection<Company> Companies { get; set; }
    }
}
