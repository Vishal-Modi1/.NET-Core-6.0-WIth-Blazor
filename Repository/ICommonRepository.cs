using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository
{
    public interface ICommonRepository
    {
        List<DropDownValues> ListDropDownValues();
    }
}
