using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using DataModels.VM.Common;


namespace Service
{
    public class AircraftCategoryService : BaseService, IAircraftCategoryService
    {
        private readonly IAircraftCategoryRepository _aircraftCategoryRepository;

        public AircraftCategoryService(IAircraftCategoryRepository aircraftCategoryRepository)
        {
            _aircraftCategoryRepository = aircraftCategoryRepository;
        }

        public CurrentResponse Create(AircraftCategory aircraftCategory)
        {
            try
            {
                bool isAircraftCategoryExist = IsAircraftCategoryExist(aircraftCategory);

                if (isAircraftCategoryExist)
                {
                    CreateResponse(aircraftCategory, HttpStatusCode.Ambiguous, "Aircraft category is already exist");
                }
                else
                {
                    aircraftCategory = _aircraftCategoryRepository.Create(aircraftCategory);
                    CreateResponse(aircraftCategory, HttpStatusCode.OK, "Aircraft category added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsAircraftCategoryExist(AircraftCategory aircraftCategory)
        {
            AircraftCategory aircraftCategoryInfo = _aircraftCategoryRepository.FindByCondition(p => p.Name == aircraftCategory.Name && p.Id != aircraftCategory.Id);

            if (aircraftCategoryInfo == null)
            {
                return false;
            }

            return true;
        }

        public CurrentResponse List()
        {
            throw new NotImplementedException();
        }
    }
}
