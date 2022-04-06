using DataModels.VM.Common;
using System.Collections.Generic;
using DataModels.VM.BillingHistory;
using DataModels.Entities;

namespace Repository.Interface
{
    public interface IBillingHistoryRepository
    {
        List<BillingHistoryDataVM> List(BillingHistoryDatatableParams datatableParams);

        BillingHistory Create(BillingHistory billingHistory);
    }
}
