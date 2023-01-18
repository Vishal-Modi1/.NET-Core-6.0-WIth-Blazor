using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class AirTrafficControlCenterService : BaseService, IAirTrafficControlCenterService
    {
        private readonly IAirTrafficControlCenterRepository _airTrafficControlCenterRepository;

        public AirTrafficControlCenterService(IAirTrafficControlCenterRepository airTrafficControlCenterRepository)
        {
            _airTrafficControlCenterRepository = airTrafficControlCenterRepository;
        }

        public CurrentResponse ListAll()
        {
            try
            {
                List<AirTrafficControlCenter> airTrafficControlCenters = _airTrafficControlCenterRepository.ListAll();

                CreateResponse(airTrafficControlCenters, HttpStatusCode.OK, "");

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
