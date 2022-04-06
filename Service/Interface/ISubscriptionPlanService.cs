using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;

namespace Service.Interface
{
    public interface ISubscriptionPlanService
    {
        CurrentResponse Create(SubscriptionPlanVM subscriptionPlan);

        CurrentResponse Edit(SubscriptionPlanVM subscriptionPlan);

        CurrentResponse List(SubscriptionDataTableParams datatableParams);

        CurrentResponse Delete(long id, long deletedBy);

        CurrentResponse UpdateActiveStatus(long id, bool isActive);
        CurrentResponse BuyPlan(int id, long userId);

        CurrentResponse GetDetails(int id);
    }
}
