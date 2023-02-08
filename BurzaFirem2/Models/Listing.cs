using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BurzaFirem2.Models
{
    public class Listing
    {
        public int ListingId { get; set; }
        public string Name { get; set; } = String.Empty;
        [JsonIgnore]
        public ICollection<Company> Companies { get; set; } = new List<Company>();
        public bool Visible { get; set; } = true;
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
