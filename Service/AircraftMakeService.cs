using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.AircraftMake;

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

        public CurrentResponse List(DatatableParams datatableParams)
        {
            try
            {
                List<AircraftMakeDataVM> aircraftMakeList = _aircraftMakeRepository.List(datatableParams);
                CreateResponse(aircraftMakeList, HttpStatusCode.OK, "");

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
                List<DropDownValues> aircraftMakeList = _aircraftMakeRepository.ListDropDownValues();
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

        public CurrentResponse Delete(int id)
        {
            try
            {
                bool isAlreadyUsed = _aircraftMakeRepository.IsAlreadyUsed(id);

                if(isAlreadyUsed)
                {
                    CreateResponse(true, HttpStatusCode.BadRequest, "Aircraft make can't be deleted because it is used in aircraft details!");
                    return _currentResponse;
                }

                _aircraftMakeRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft make deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(AircraftMake aircraftMake)
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
                    aircraftMake = _aircraftMakeRepository.Edit(aircraftMake);
                    CreateResponse(aircraftMake, HttpStatusCode.OK, "Aircraft make updated successfully");
                }

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
