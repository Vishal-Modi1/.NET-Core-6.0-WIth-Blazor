﻿using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.SubscriptionPlan;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class SubscriptionPlanService : BaseService, ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IBillingHistoryRepository _billingHistoryRepository;

        public SubscriptionPlanService(ISubscriptionPlanRepository subscriptionPlanRepository,
            IBillingHistoryRepository billingHistoryRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _billingHistoryRepository = billingHistoryRepository;
        }

        public CurrentResponse GetDetails(int id)
        {
            SubscriptionPlan subscriptionPlan = _subscriptionPlanRepository.FindByCondition(p => p.Id == id);

            SubscriptionPlanVM subscriptionPlanVM = new SubscriptionPlanVM();

            if (subscriptionPlan != null)
            {
                subscriptionPlanVM = ToBusinessObject(subscriptionPlan);
            }

            CreateResponse(subscriptionPlanVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }
  
        public CurrentResponse Create(SubscriptionPlanVM subscriptionPlanVM)
        {
            SubscriptionPlan subscriptionPlan = ToDataObject(subscriptionPlanVM);

            try
            {
                bool isPlanNameExist = IsPlanNameExist(subscriptionPlan);

                if (isPlanNameExist)
                {
                    CreateResponse(subscriptionPlan, HttpStatusCode.Ambiguous, "Subscription plan name is already exist");
                    return _currentResponse;
                }

                bool isPlanExist = IsPlanAlreadyExist(subscriptionPlan);

                if (isPlanExist)
                {
                    CreateResponse(subscriptionPlan, HttpStatusCode.Ambiguous, "Subscription plan is already exist");
                }
                else
                {
                    subscriptionPlan = _subscriptionPlanRepository.Create(subscriptionPlan);
                    CreateResponse(subscriptionPlan, HttpStatusCode.OK, "Subscription plan added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateActiveStatus(long id, bool isActive)
        {
            try
            {
                _subscriptionPlanRepository.UpdateActiveStatus(id, isActive);

                string message = "Subscription Plan activated successfully.";

                if (!isActive)
                {
                    message = "Subscription Plan deactivated successfully.";

                }

                CreateResponse(true, HttpStatusCode.OK, message);

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _subscriptionPlanRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Subscription plan deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(SubscriptionPlanVM subscriptionPlanVM)
        {
            SubscriptionPlan subscriptionPlan = ToDataObject(subscriptionPlanVM);

            try
            {
                bool isPlanNameExist = IsPlanNameExist(subscriptionPlan);

                if(isPlanNameExist)
                {
                    CreateResponse(subscriptionPlan, HttpStatusCode.Ambiguous, "Subscription plan name is already exist");
                    return _currentResponse;
                }

                bool isPlanExist = IsPlanAlreadyExist(subscriptionPlan);

                if (isPlanExist)
                {
                    CreateResponse(subscriptionPlan, HttpStatusCode.Ambiguous, "Subscription plan is already exist");
                }
                else
                {
                    subscriptionPlan = _subscriptionPlanRepository.Edit(subscriptionPlan);
                    CreateResponse(subscriptionPlan, HttpStatusCode.OK, "Subscription plan updated successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(SubscriptionDataTableParams datatableParams)
        {
            try
            {
                List<SubscriptionPlanDataVM> subscriptionPlanslist = _subscriptionPlanRepository.List(datatableParams);

                CreateResponse(subscriptionPlanslist, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsPlanAlreadyExist(SubscriptionPlan subscriptionPlan)
        {
            bool isPlanExist = _subscriptionPlanRepository.IsPlanAlreadyExist(subscriptionPlan);

            return isPlanExist;
        }

        private bool IsPlanNameExist(SubscriptionPlan subscriptionPlan)
        {
            bool isPlanNameExist = _subscriptionPlanRepository.IsPlanNameAlreadyExist(subscriptionPlan);

            return isPlanNameExist;
        }

        public CurrentResponse BuyPlan(int id, long userId)
        {
            try
            {
                BillingHistory billingHistory = GenerateBillingDetails(id, userId);

                CreateResponse(billingHistory, HttpStatusCode.OK, "Subscription Plan Activated.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private BillingHistory GenerateBillingDetails(int id, long userId)
        {
            SubscriptionPlan subscriptionPlan = _subscriptionPlanRepository.FindByCondition(p => p.Id == id);

            BillingHistory billingHistory = new BillingHistory();

            billingHistory.SubscriptionPlanName = subscriptionPlan.Name;
            billingHistory.UserId = userId;
            billingHistory.ModuleIds = subscriptionPlan.ModuleIds;
            billingHistory.IsCombo = subscriptionPlan.IsCombo;
            billingHistory.Price = subscriptionPlan.Price;
            billingHistory.Duration = subscriptionPlan.Duration;
            billingHistory.IsActive = true;

            billingHistory.PlanStartDate = DateTime.UtcNow;
            billingHistory.PlanEndDate = billingHistory.PlanStartDate.AddMonths(billingHistory.Duration);

            billingHistory.CreatedOn = DateTime.UtcNow;

            _billingHistoryRepository.Create(billingHistory);

            return billingHistory;
        }

        #region Obbject Mapper

        public SubscriptionPlan ToDataObject(SubscriptionPlanVM subscriptionPlanVM)
        {
            SubscriptionPlan subscriptionPlan = new SubscriptionPlan();

            subscriptionPlan.Name = subscriptionPlanVM.Name;
            subscriptionPlan.Id = subscriptionPlanVM.Id;
            subscriptionPlan.Duration = subscriptionPlanVM.Duration;
            subscriptionPlan.Price = subscriptionPlanVM.Price;
            subscriptionPlan.ModuleIds = subscriptionPlanVM.ModuleIds;
            subscriptionPlan.IsCombo = subscriptionPlanVM.ModuleIds.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Length > 1;
            subscriptionPlan.IsActive = true;
            subscriptionPlan.Description = subscriptionPlanVM.Description;

            subscriptionPlan.CreatedBy = subscriptionPlanVM.CreatedBy;

            if (subscriptionPlanVM.Id == 0)
            {
                subscriptionPlan.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                subscriptionPlan.UpdatedOn = DateTime.UtcNow;
                subscriptionPlan.UpdatedBy = subscriptionPlanVM.UpdatedBy;
            }

            return subscriptionPlan;
        }

        private SubscriptionPlanVM ToBusinessObject(SubscriptionPlan subscriptionPlan)
        {
            SubscriptionPlanVM subscriptionPlanVM = new SubscriptionPlanVM();

            subscriptionPlanVM.Name = subscriptionPlan.Name;
            subscriptionPlanVM.Id = subscriptionPlan.Id;
            subscriptionPlanVM.Duration = subscriptionPlan.Duration;
            subscriptionPlanVM.Price = subscriptionPlan.Price;
            subscriptionPlanVM.ModuleIds = subscriptionPlan.ModuleIds;
            subscriptionPlanVM.Description = subscriptionPlan.Description;

            return subscriptionPlanVM;
        }

        #endregion
    }
}
