using System.ComponentModel.DataAnnotations;

namespace BurzaFirem2.InputModels
{
    public class CompanyIM
    {
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Wanted { get; set; } = String.Empty;
        public string Offer { get; set; } = String.Empty;
        public string AddressStreet { get; set; } = String.Empty;
        public string Municipality { get; set; } = String.Empty;
        public string CompanyBranches { get; set; } = String.Empty;
        public string CompanyUrl { get; set; } = String.Empty;
        public string PresentationUrl { get; set; } = String.Empty;
    }
}
