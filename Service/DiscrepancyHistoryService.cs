using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class DiscrepancyHistoryService : BaseService, IDiscrepancyHistoryService
    {
        private readonly IDiscrepancyHistoryRepository _discrepancyHistoryRepository;

        public DiscrepancyHistoryService(IDiscrepancyHistoryRepository discrepancyHistoryRepository)
        {
            _discrepancyHistoryRepository = discrepancyHistoryRepository;
        }

        public CurrentResponse List(long discrepancyId)
        {
            try
            {
                List<DiscrepancyHistoryVM> discrepancyHistories = _discrepancyHistoryRepository.List(discrepancyId);

                CreateResponse(discrepancyHistories, HttpStatusCode.OK, "");

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
