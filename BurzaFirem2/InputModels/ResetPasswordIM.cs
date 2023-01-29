namespace BurzaFirem2.InputModels
{
    public class ResetPasswordIM
    {
        public string Email { get; set; } = String.Empty;
        public string Code { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
    }
}
