using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IDiscrepancyHistoryService
    {
        CurrentResponse List(long discrepancyId);
    }
}
