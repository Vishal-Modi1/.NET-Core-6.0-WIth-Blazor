using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;

namespace DataModels.VM.User
{
    public class UserVM : CommonField
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool IsSendEmailInvite { get; set; }

        public bool IsSendTextMessage { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter 10 digits no")]
        public string Phone { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select role")]
        public int RoleId { get; set; }

        public string Role { get; set; }

        public bool IsInstructor { get; set; }

        public Nullable<int> InstructorTypeId { get; set; }

        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Please select company")]
        public int? CompanyId { get; set; }
        public string ExternalId { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string Gender { get; set; }
        [NotMapped]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Please select country")]
        public Nullable<int> CountryId { get; set; }
        public string Country { get; set; }
        public string ImageName { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [NotMapped]
        public string ActivationLink { get; set; }
        [NotMapped]
        public List<DropDownValues> UserRoles { get; set; }
        [NotMapped]
        public List<DropDownValues> InstructorTypes { get; set; }
        [NotMapped]
        public List<DropDownValues> Countries { get; set; }
        [NotMapped]
        public List<DropDownValues> Companies { get; set; }

        [NotMapped]
        public List<UserPreferenceVM> UserPreferences { get; set; }

        [NotMapped]
        public bool IsFromMyProfile { get; set; }

        [NotMapped]
        public bool IsInvited { get; set; }

        [NotMapped]
        public new long? CreatedBy { get; set; }
    }
}
