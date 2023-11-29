using System.Text.Json.Serialization;

namespace BurzaFirem2.Models
{
    public class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        [JsonIgnore]
        public Company? Company { get; set; }
        public int CompanyId { get; set; }
    }
}
