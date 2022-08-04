using DataModels.VM.Common;
using System.Collections.Generic;

namespace DataModels.VM.User
{
    public class InviteUserVM
    {
        public long Id { get; set; }
        public string Email { get; set; }

        public int CompanyId { get; set; }

        public List<DropDownValues> ListCompanies { get; set; }

        public List<DropDownValues> ListRoles { get; set; } 

        public int RoleId { get; set; }

        public long InvitedBy { get; set; }

        public string ActivationLink { get; set; }  
    }
}
