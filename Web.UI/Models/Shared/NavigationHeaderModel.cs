using DataModels.VM.Common;
using DataModels.VM.Company;
using DataModels.VM.User;

namespace Web.UI.Models.Shared
{
    public class NavigationHeaderModel
    {
        public UserVM User { get; set; }

        public CompanyVM Company { get; set; }

        public List<DropDownValues> CompanyList { get; set; }
    }
}
