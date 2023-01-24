using BurzaFirem2.Models;

namespace BurzaFirem2.Emails.ViewModels
{
    public class ResetPasswordVM
    {
        public string ConfirmationCode { get; set; } = String.Empty;
        public ApplicationUser User { get; set; }
        public string ConfirmEmailUrl { get; set; } = String.Empty;
        public string AppUrl { get; set; } = String.Empty;
    }
}
