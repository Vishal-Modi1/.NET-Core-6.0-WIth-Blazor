using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;

namespace Service.Interface
{
    public interface ISubscriptionPlanService
    {
        CurrentResponse Create(SubscriptionPlanVM subscriptionPlan);

        CurrentResponse Edit(SubscriptionPlanVM subscriptionPlan);

        CurrentResponse List(DatatableParams datatableParams);

        CurrentResponse Delete(long id, long deletedBy);

        CurrentResponse UpdateActiveStatus(long id, bool isActive);

        CurrentResponse GetDetails(int id);
    }
}
