namespace BurzaFirem2.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public DateTime Updated { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
