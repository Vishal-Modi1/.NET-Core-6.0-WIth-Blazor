using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.BillingConfigurations;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using System.Collections.Generic;

namespace Service
{
    public class BillingConfigurationService : BaseService, IBillingConfigurationService
    {
        private readonly IBillingConfigurationRepository _BillingConfigurationRepository;

        public BillingConfigurationService(IBillingConfigurationRepository BillingConfigurationRepository)
        {
            _BillingConfigurationRepository = BillingConfigurationRepository;
        }

        public CurrentResponse FindByUserId(int companyId)
        {
            try
            {
                BillingConfigurationVM billingConfigurationVM = GetDetails(companyId);
                CreateResponse(billingConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(0, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(BillingConfiguration billingConfiguration)
        {
            try
            {
                _BillingConfigurationRepository.SetDefault(billingConfiguration);

                BillingConfigurationVM billingConfigurationVM = GetDetails(billingConfiguration.CompanyId);

                CreateResponse(billingConfigurationVM, HttpStatusCode.OK, "Billing value updated");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private BillingConfigurationVM GetDetails(int companyId)
        {
            BillingConfiguration billingConfiguration = _BillingConfigurationRepository.FindByCondition(p => p.CompanyId == companyId);
            BillingConfigurationVM billingConfigurationVM = new BillingConfigurationVM();

            if (billingConfiguration == null)
            {
                billingConfiguration = new BillingConfiguration();
            }
            else
            {
                billingConfigurationVM.Id = billingConfiguration.Id;
                billingConfigurationVM.CompanyId = billingConfiguration.CompanyId;
                billingConfigurationVM.BillingFollows = billingConfiguration.BillingFollows;
            }

            billingConfigurationVM.BillingFollowsList = new List<DropDownStringValues>();

            billingConfigurationVM.BillingFollowsList.Add(new DropDownStringValues() { Id = "Hobbs", Name = "Hobbs" });
            billingConfigurationVM.BillingFollowsList.Add(new DropDownStringValues() { Id = "Tach", Name = "Tach" });


            return billingConfigurationVM;
        }
    }
}
