using DataModels.VM.Common;
using System.Collections.Generic;

namespace DataModels.VM.LogBook
{
    public class LogBookFilterVM : CommonFilterVM
    {
        public List<DropDownLargeValues> UsersList { get; set; } = new();
        public long UserId { get; set; }
    }
}
