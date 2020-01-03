using System.ComponentModel.DataAnnotations;

namespace Im.Access.Portal.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
