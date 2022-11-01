using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IDiscrepancyStatusRepository
    {
        List<DropDownValues> ListDropDownValues();
    }
}
