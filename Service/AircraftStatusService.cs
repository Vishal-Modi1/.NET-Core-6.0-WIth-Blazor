using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class AircraftStatusService : BaseService, IAircraftStatusService
    {
        private readonly IAircraftStatusRepository _aircraftStatusRepository;

        public AircraftStatusService(IAircraftStatusRepository aircraftStatusRepository)
        {
            _aircraftStatusRepository = aircraftStatusRepository;
        }

        public CurrentResponse ListAll()
        {
            try
            {
                List<AircraftStatus> aircraftStatusList = _aircraftStatusRepository.ListAll();

                CreateResponse(aircraftStatusList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListDropDownValues()
        {
            try
            {
                List<DropDownValues> aircraftStatusList = _aircraftStatusRepository.ListDropDownValues();

                CreateResponse(aircraftStatusList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetById(byte id)
        {
            try
            {
                AircraftStatus aircraftStatus = _aircraftStatusRepository.GetById(id);

                CreateResponse(aircraftStatus, HttpStatusCode.OK, "");

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
