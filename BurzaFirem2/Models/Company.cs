using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BurzaFirem2.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        public StoredImage? Logo { get; set; } = null;
        public string? LogoId { get; set; } = null;
        public string? Description { get; set; } = String.Empty;
        public string? Wanted { get; set; } = String.Empty;
        public string? Offer { get; set; } = String.Empty;
        public string AddressStreet { get; set; } = String.Empty;
        public string Municipality { get; set; } = String.Empty;
        public string? CompanyUrl { get; set; } = String.Empty;
        public string? PresentationUrl { get; set; } = String.Empty;
        public string? CompanyBranches { get; set; } = String.Empty;
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<StoredImage> Images { get; set; } = new List<StoredImage>();
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();

        [Required]
        public Guid UserId { get; set; } 
        [Required]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Updated { get; set; }
    }
}
