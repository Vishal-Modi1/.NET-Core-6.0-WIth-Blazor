using DataModels.VM.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataModels.VM.User
{
    public class InviteUserVM
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select company")]
        public int CompanyId { get; set; }

        public List<DropDownValues> ListCompanies { get; set; }

        public List<DropDownValues> ListRoles { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select role")]
        public int RoleId { get; set; }

        public long InvitedBy { get; set; }

        public string ActivationLink { get; set; }  
    }
}
