using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IDiscrepancyStatusRepository: IBaseRepository<DiscrepancyStatus>
    {
        List<DropDownValues> ListDropDownValues();
    }
}
