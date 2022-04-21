using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface ICommonRepository
    {
        List<DropDownValues> ListDropDownValues();
    }
}
