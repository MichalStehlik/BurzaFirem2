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
        public string LogoFileName { get; set; } = String.Empty;
        public ImageOrientation LogoOrientation { get; set; } = ImageOrientation.Unknown;
        public string Description { get; set; } = String.Empty;
        public string Wanted { get; set; } = String.Empty;
        public string Offer { get; set; } = String.Empty;
        public string AddressStreet { get; set; } = String.Empty;
        public string Municipality { get; set; } = String.Empty;
        public string CompanyUrl { get; set; } = String.Empty;
        public string PresentationUrl { get; set; } = String.Empty;
        public string CompanyBranches { get; set; } = String.Empty;
        public ICollection<Branch> Branches { get; set; } = new List<Branch>();
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        [Required]
        public string UserId { get; set; }
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
