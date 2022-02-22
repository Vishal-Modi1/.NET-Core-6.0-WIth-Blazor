using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using DataModels.VM.Common;

namespace Service
{
    public class AircraftClassService : BaseService, IAircraftClassService
    {
        private readonly IAircraftClassRepository _aircraftClassRepository;

        public AircraftClassService(IAircraftClassRepository aircraftClassRepository)
        {
            _aircraftClassRepository = aircraftClassRepository;
        }

        public CurrentResponse Create(AircraftClass aircraftClass)
        {
            try
            {
                bool isAircraftClassExist = IsAircraftClassExist(aircraftClass);

                if (isAircraftClassExist)
                {
                    CreateResponse(aircraftClass, HttpStatusCode.Ambiguous, "Aircraft class is already exist");
                }
                else
                {
                    aircraftClass = _aircraftClassRepository.Create(aircraftClass);
                    CreateResponse(aircraftClass, HttpStatusCode.OK, "Aircraft class added successfully");
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
            throw new NotImplementedException();
        }

        private bool IsAircraftClassExist(AircraftClass aircraftClass)
        {
            AircraftClass aircraftClassInfo = _aircraftClassRepository.FindByCondition(p => p.Name == aircraftClass.Name && p.Id != aircraftClass.Id);

            if (aircraftClassInfo == null)
            {
                return false;
            }

            return true;
        }
    }
}
