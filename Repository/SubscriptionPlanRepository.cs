using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private MyContext _myContext;

        public SubscriptionPlan Create(SubscriptionPlan subscriptionPlan)
        {
            using (_myContext = new MyContext())
            {
                _myContext.SubscriptionPlans.Add(subscriptionPlan);
                _myContext.SaveChanges();

                return subscriptionPlan;
            }
        }

        public SubscriptionPlan Edit(SubscriptionPlan subscriptionPlan)
        {
            using (_myContext = new MyContext())
            {
                SubscriptionPlan existingDetails = _myContext.SubscriptionPlans.Where(p => p.Id == subscriptionPlan.Id).FirstOrDefault();

                if (existingDetails == null)
                    return subscriptionPlan;

                existingDetails.Price = subscriptionPlan.Price;
                existingDetails.Name = subscriptionPlan.Name;
                existingDetails.ModuleIds = subscriptionPlan.ModuleIds;
                existingDetails.IsCombo = subscriptionPlan.IsCombo;
                existingDetails.Duration = subscriptionPlan.Duration;
                existingDetails.Description = subscriptionPlan.Description;

                existingDetails.UpdatedBy = subscriptionPlan.UpdatedBy;
                existingDetails.UpdatedOn = subscriptionPlan.UpdatedOn;

                _myContext.SaveChanges();

                return subscriptionPlan;
            }
        }

        public List<SubscriptionPlanDataVM> List(SubscriptionDataTableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                List<SubscriptionPlanDataVM> list;

                string sql = "";

                if (datatableParams.IsActive.HasValue)
                {
                    sql = $"EXEC dbo.GetSubscriptionPlanList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length}," +
                        $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', {datatableParams.IsActive} ";
                }
                else
                {
                    sql = $"EXEC dbo.GetSubscriptionPlanList '{ datatableParams.SearchText }', { datatableParams.Start }, {datatableParams.Length}," +
                        $"'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";
                }

                list = _myContext.SubscriptionPlanData.FromSqlRaw<SubscriptionPlanDataVM>(sql).ToList();

                return list;
            }
        }

        public void Delete(long id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                SubscriptionPlan subscriptionPlan = _myContext.SubscriptionPlans.Where(p => p.Id == id).FirstOrDefault();

                if (subscriptionPlan != null)
                {
                    subscriptionPlan.IsDeleted = true;
                    subscriptionPlan.DeletedOn = DateTime.UtcNow;
                    subscriptionPlan.DeletedBy = deletedBy;

                    _myContext.SaveChanges();
                }
            }
        }

        public bool IsPlanAlreadyExist(SubscriptionPlan subscriptionPlan)
        {
            using (_myContext = new MyContext())
            {
                bool isPlanExist = _myContext.SubscriptionPlans.Any(p => p.Price == subscriptionPlan.Price && p.Name == subscriptionPlan.Name && p.Duration == subscriptionPlan.Duration && p.Id != subscriptionPlan.Id);

                return isPlanExist;
            }
        }

        public bool IsPlanNameAlreadyExist(SubscriptionPlan subscriptionPlan)
        {
            using (_myContext = new MyContext())
            {
                bool isPlanNameExist = _myContext.SubscriptionPlans.Any(p =>  p.Name == subscriptionPlan.Name && p.Id != subscriptionPlan.Id);

                return isPlanNameExist;
            }
        }

        public SubscriptionPlan FindByCondition(Expression<Func<SubscriptionPlan, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                SubscriptionPlan existingPlan = _myContext.SubscriptionPlans.Where(predicate).FirstOrDefault();

                return existingPlan;
            }
        }

        public void UpdateActiveStatus(long id, bool isActive)
        {
            using (_myContext = new MyContext())
            {
                SubscriptionPlan existingPlan = _myContext.SubscriptionPlans.Where(p=> p.Id == id).FirstOrDefault();

                if (existingPlan != null)
                {
                    existingPlan.IsActive = isActive;
                    _myContext.SaveChanges();
                }
            }
        }
    }
}
