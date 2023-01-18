using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IDiscrepancyRepository : IBaseRepository<Discrepancy>
    {
        Discrepancy Edit(Discrepancy discrepancy);

        List<DiscrepancyDataVM> List(DiscrepancyDatatableParams datatableParams);
    }
}
