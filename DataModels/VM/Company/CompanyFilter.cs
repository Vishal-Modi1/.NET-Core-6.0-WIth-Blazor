using DataModels.VM.Common;
using System.Collections.Generic;

namespace DataModels.VM.Company
{
    public class CompanyFilter
    {
        public List<DropDownValues> Cities { get; set; } = new();
        public List<DropDownValues> States { get; set; } = new();
    }
}
