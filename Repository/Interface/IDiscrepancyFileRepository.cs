using DataModels.Entities;
using DataModels.VM.Discrepancy;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IDiscrepancyFileRepository : IBaseRepository<DiscrepancyFile>
    {
        DiscrepancyFile Edit(DiscrepancyFile discrepancyFile);

        void Delete(long id);

        List<DiscrepancyFileVM> List(long discrepancyId);

        bool UpdateDocumentName(long id, string name);
    }
}
