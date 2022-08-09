using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.Account
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        public string ResetURL { get; set; }
    }
}
