using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IBillingHistoryService
    {
        CurrentResponse List(DatatableParams datatableParams);
    }
}
