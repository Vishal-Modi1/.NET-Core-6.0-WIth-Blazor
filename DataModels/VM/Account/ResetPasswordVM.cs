using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Account
{
    public class ResetPasswordVM
    {
        public string Token { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "The confirm password field is required.")]
        [Compare("Password", ErrorMessage = "The confirm password does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
