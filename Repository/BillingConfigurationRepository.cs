using DataModels.Entities;
using Repository.Interface;
using System.Linq;

namespace Repository
{
    public class BillingConfigurationRepository : BaseRepository<BillingConfiguration>, IBillingConfigurationRepository
    {
        private readonly MyContext _myContext;

        public BillingConfigurationRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public void SetDefault(BillingConfiguration billingConfiguration)
        {
            BillingConfiguration existingBillingConfiguration = _myContext.BillingConfigurations.Where(p => p.CompanyId == billingConfiguration.CompanyId).FirstOrDefault();

            if (existingBillingConfiguration != null)
            {
                existingBillingConfiguration.BillingFollows = billingConfiguration.BillingFollows;
            }
            else
            {
                _myContext.BillingConfigurations.Add(billingConfiguration);
            }
         
            _myContext.SaveChanges();
        }
    }
}
