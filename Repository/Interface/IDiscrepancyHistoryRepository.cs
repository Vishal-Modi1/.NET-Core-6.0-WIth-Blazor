using DataModels.Entities;
using DataModels.VM.Discrepancy;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IDiscrepancyHistoryRepository : IBaseRepository<DiscrepancyHistory>
    {
        List<DiscrepancyHistoryVM> List(long discrepancyId);
    }
}
