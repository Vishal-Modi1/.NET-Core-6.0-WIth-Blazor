using System;
using System.Collections.Generic;
using DataModels.VM.Common;

namespace DataModels.VM.User
{
    public class UserVM
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string Email { get; set; }

        public bool IsSendEmailInvite { get; set; }

        public bool IsSendTextMessage { get; set; }

        public string Phone { get; set; }

        public int RoleId { get; set; }
        public string Role { get; set; }

        public bool IsInstructor { get; set; }

        public Nullable<int> InstructorTypeId { get; set; }

        public string CompanyName { get; set; }

        public int? CompanyId { get; set; }

        public string ExternalId { get; set; }

        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string Gender { get; set; }

        public int GenderId { get; set; }

        public Nullable<int> CountryId { get; set; }
        public string Country { get; set; }
        public string ImageName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<long> UpdatedBy { get; set; }

        public string ActivationLink { get; set; }

        public List<DropDownValues> UserRoles { get; set; }
        public List<DropDownValues> InstructorTypes { get; set; }
        public List<DropDownValues> Countries { get; set; }
        public List<DropDownValues> Companies { get; set; }

        public bool IsFromMyProfile { get; set; }
    }
}
