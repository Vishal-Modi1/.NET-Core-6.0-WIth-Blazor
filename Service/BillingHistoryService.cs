using DataModels.VM.Common;
using Service.Interface;
using System;
using Repository.Interface;
using System.Collections.Generic;
using DataModels.VM.BillingHistory;
using System.Net;

namespace Service
{
    public class BillingHistoryService : BaseService, IBillingHistoryService
    {
        private readonly IBillingHistoryRepository _billingHistoryRepository;

        public BillingHistoryService(IBillingHistoryRepository billingHistoryRepository)
        {
            _billingHistoryRepository = billingHistoryRepository;
        }

        public CurrentResponse List(BillingHistoryDatatableParams datatableParams)
        {
            try
            {
                List<BillingHistoryDataVM> companyList = _billingHistoryRepository.List(datatableParams);

                CreateResponse(companyList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
