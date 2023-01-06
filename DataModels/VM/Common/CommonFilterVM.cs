using System.Collections.Generic;

namespace DataModels.VM.Common
{
    public class CommonFilterVM
    {
        public int CompanyId { get; set; }

        public bool IsArchive { get; set; }
        public List<DropDownValues> Companies { get; set; } = new();
        public List<DropDownValues> UserRoles { get; set; } = new();
    }
}
