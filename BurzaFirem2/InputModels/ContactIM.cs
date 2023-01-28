namespace BurzaFirem2.InputModels
{
    public class ContactIM
    {
        public int ContactId { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public int CompanyId { get; set; }
    }
}
