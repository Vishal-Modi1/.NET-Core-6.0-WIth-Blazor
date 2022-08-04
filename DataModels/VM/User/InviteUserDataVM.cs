using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.VM.User
{
    public class InviteUserDataVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public long InvitedBy { get; set; }
        public DateTime InvitedOn { get; set; }
        public int TotalRecords { get; set; }
        [NotMapped]
        public bool IsLoadingEditButton { get; set; }
    }
}
