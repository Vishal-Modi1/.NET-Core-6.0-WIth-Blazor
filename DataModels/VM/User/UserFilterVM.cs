using DataModels.VM.Company;

namespace DataModels.VM.User
{
    public class UserFilterVM : CompanyFilterVM
    {
        public int RoleId { get; set; }
    }
}
