using DataModels.VM.Common;

namespace DataModels.VM.User
{
    public class UserFilterVM : CommonFilterVM
    {
        public int RoleId { get; set; }

        public bool IsArchive { get; set; }
    }
}
