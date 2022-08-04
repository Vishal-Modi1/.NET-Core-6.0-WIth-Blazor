using System;

namespace DataModels.Entities
{
    public  class InviteUser
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public int CompanyId { get; set; }
        public long InvitedBy { get; set; }
        public long? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime InvitedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

    }
}
