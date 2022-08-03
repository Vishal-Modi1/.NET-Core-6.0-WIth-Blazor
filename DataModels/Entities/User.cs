using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModels.Entities
{
    public class User : CommonField
    {
        public long Id { get; set; }

        public string ImageName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsSendEmailInvite { get; set; }
        public bool IsSendTextMessage { get; set; }
        public string Phone { get; set; }

        public bool IsInstructor { get; set; }
        public Nullable<int> InstructorTypeId { get; set; }
        public string ExternalId { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string Gender { get; set; }
        public Nullable<int> CountryId { get; set; }

        [NotMapped]
        public Nullable<int> CompanyId { get; set; }

        [NotMapped]
        public string CompanyName { set; get; }

        [NotMapped]
        public int RoleId { get; set; }

        [NotMapped]
        public string RoleName { get; set; }
        public string Password { get; set; }

        public new long? CreatedBy { get; set; }

    }
}
