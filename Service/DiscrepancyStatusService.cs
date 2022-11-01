using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class DiscrepancyStatusService : BaseService, IDiscrepancyStatusService
    {
        private readonly IDiscrepancyStatusRepository _discrepancyStatusRepository;

        public DiscrepancyStatusService(IDiscrepancyStatusRepository discrepancyStatusRepository)
        {
            _discrepancyStatusRepository = discrepancyStatusRepository;
        }

        public CurrentResponse ListDropDownValues()
        {
            try
            {
                List<DropDownValues> discepancyStatusesList = _discrepancyStatusRepository.ListDropDownValues();

                CreateResponse(discepancyStatusesList, HttpStatusCode.OK, "");

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
