using System.Text.Json.Serialization;

namespace BurzaFirem2.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }
        public string Name { get; set; } = String.Empty;
        [JsonIgnore]
        public ICollection<Company> Companies { get; set; }
    }
}
