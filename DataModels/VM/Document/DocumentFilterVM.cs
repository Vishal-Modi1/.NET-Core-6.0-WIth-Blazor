using DataModels.VM.Common;
using DataModels.VM.Company;
using System.Collections.Generic;

namespace DataModels.VM.Document
{
    public class DocumentFilterVM : CommonFilterVM
    {
        public int ModuleId { get; set; }

        public List<DropDownValues> ModulesList { get; set; }
    }
}
