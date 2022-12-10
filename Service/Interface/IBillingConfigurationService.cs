using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IBillingConfigurationService
    {
        CurrentResponse FindByUserId(int companyId);

        CurrentResponse SetDefault(BillingConfiguration billingConfiguration);
    }
}
