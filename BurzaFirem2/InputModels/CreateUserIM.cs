namespace BurzaFirem2.InputModels
{
    public class CreateUserIM
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public bool Admin { get; set; } = false;
        public bool Editor { get; set; } = false;
    }
}
