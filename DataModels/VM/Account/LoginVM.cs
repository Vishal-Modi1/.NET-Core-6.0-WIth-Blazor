using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "The email field is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The password field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
