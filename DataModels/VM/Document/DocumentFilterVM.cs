using DataModels.VM.Common;
using System.Collections.Generic;

namespace DataModels.VM.Document
{
    public class DocumentFilterVM : CommonFilterVM
    {
        public int ModuleId { get; set; }

        public List<DropDownValues> ModulesList { get; set; } = new();

        public List<DropDownValues> TypesList { get; set; } = new();

        public List<DropDownLargeValues> UsersList { get; set; } = new();

        public long UserId { get; set; }

        public string DocumentType { get; set; }
    }
}
