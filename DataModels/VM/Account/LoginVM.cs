using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public string TimeZone { get; set; }
        public bool RememberMe { get; set; }
    }
}
