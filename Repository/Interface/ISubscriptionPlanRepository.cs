using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repository.Interface
{
    public interface ISubscriptionPlanRepository
    {
        SubscriptionPlan Create(SubscriptionPlan subscriptionPlan);

        SubscriptionPlan Edit(SubscriptionPlan subscriptionPlan);

        List<SubscriptionPlanDataVM> List(SubscriptionDataTableParams datatableParams);
        
        void Delete(long id, long deletedBy);

        bool IsPlanAlreadyExist(SubscriptionPlan subscriptionPlan);

        bool IsPlanNameAlreadyExist(SubscriptionPlan subscriptionPlan);

        SubscriptionPlan FindByCondition(Expression<Func<SubscriptionPlan, bool>> predicate);

        void UpdateActiveStatus(long id, bool isActive);
    }
}
