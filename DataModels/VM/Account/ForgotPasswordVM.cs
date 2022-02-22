using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Account
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "The email field is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public string ResetURL { get; set; }
    }
}
