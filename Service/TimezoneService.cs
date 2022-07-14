using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class TimezoneService : BaseService, ITimezoneService
    {
        private readonly ITimezoneRepository _timezoneRepository;

        public TimezoneService(ITimezoneRepository timezoneRepository)
        {
            _timezoneRepository = timezoneRepository;
        }

        public CurrentResponse ListDropdownValues()
        {
            try
            {
                List<DropDownValues> timezonesList = _timezoneRepository.ListDropdownValues();
                CreateResponse(timezonesList, HttpStatusCode.OK, "");

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
