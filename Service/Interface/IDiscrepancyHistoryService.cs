using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;

namespace Service.Interface
{
    public interface IDiscrepancyHistoryService
    {
        void Create(Discrepancy oldData, Discrepancy newData);
    }
}
