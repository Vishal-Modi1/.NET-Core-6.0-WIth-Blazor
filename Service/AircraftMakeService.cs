using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;


namespace Service
{
    public class AircraftMakeService : BaseService, IAircraftMakeService
    {
        private readonly IAircraftMakeRepository _aircraftMakeRepository;

        public AircraftMakeService(IAircraftMakeRepository aircraftMakeRepository)
        {
            _aircraftMakeRepository = aircraftMakeRepository;
        }

        public CurrentResponse Create(AircraftMake aircraftMake)
        {
            try
            {
                bool isAircraftMakeExist = IsAircraftMakeExist(aircraftMake);

                if (isAircraftMakeExist)
                {
                    CreateResponse(aircraftMake, HttpStatusCode.Ambiguous, "Aircraft make is already exist");
                }
                else
                {
                    aircraftMake = _aircraftMakeRepository.Create(aircraftMake);
                    CreateResponse(aircraftMake, HttpStatusCode.OK, "Aircraft make added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List()
        {
            try
            {
                List<AircraftMake> aircraftMakeList = _aircraftMakeRepository.List();
                CreateResponse(aircraftMakeList, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsAircraftMakeExist(AircraftMake aircraftMake)
        {
            AircraftMake aircraftMakeInfo = _aircraftMakeRepository.FindByCondition(p => p.Name == aircraftMake.Name && p.Id != aircraftMake.Id);

            if (aircraftMakeInfo == null)
            {
                return false;
            }

            return true;
        }
    }
}
