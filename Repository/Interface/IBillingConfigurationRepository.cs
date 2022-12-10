using DataModels.Entities;

namespace Repository.Interface
{
    public interface IBillingConfigurationRepository : IBaseRepository<BillingConfiguration>
    {
        void SetDefault(BillingConfiguration billingConfiguration);
    }
}
