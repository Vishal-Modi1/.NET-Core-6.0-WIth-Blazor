using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.MyAccount
{
    public class ChangePasswordVM
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "The old password field is required.")]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "The new password field is required.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The confirm password field is required.")]
        [Compare("NewPassword", ErrorMessage = "The confirm password does not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
       
    }
}
