using System;
using System.Collections.Generic;
using DataModels.VM.UserRolePermission;

namespace DataModels.VM.Account
{
    public class LoginResponseVM
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int RoleId { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public string ExternalId { get; set; }
        public Nullable<System.DateTime> DateofBirth { get; set; }
        public string Gender { get; set; }
        public List<UserRolePermissionDataVM> UserPermissionList { get; set; }
    }
}
