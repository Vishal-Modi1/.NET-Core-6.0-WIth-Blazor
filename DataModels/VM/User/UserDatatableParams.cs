using DataModels.VM.Common;

namespace DataModels.VM.User
{
    public class UserDatatableParams : DatatableParams
    {
        public int RoleId { get; set; }

        public bool IsArchive { get; set; }
    }
}
