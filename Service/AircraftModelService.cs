using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.AircraftModel;

namespace Service
{
    public class AircraftModelService : BaseService, IAircraftModelService
    {
        private readonly IAircraftModelRepository _aircraftModelRepository;

        public AircraftModelService(IAircraftModelRepository aircraftModelRepository)
        {
            _aircraftModelRepository = aircraftModelRepository;
        }

        public CurrentResponse Create(AircraftModel aircraftModel)
        {
            try
            {
                bool isAircraftModelExist = IsAircraftModelExist(aircraftModel);

                if (isAircraftModelExist)
                {
                    CreateResponse(aircraftModel, HttpStatusCode.Ambiguous, "Aircraft model is already exist");
                }
                else
                {
                    aircraftModel = _aircraftModelRepository.Create(aircraftModel);
                    CreateResponse(aircraftModel, HttpStatusCode.OK, "Aircraft model added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsAircraftModelExist(AircraftModel aircraftModel)
        {
            AircraftModel aircraftModelInfo = _aircraftModelRepository.FindByCondition(p => p.Name == aircraftModel.Name && p.Id != aircraftModel.Id);

            if (aircraftModelInfo == null)
            {
                return false;
            }

            return true;
        }

        public CurrentResponse List()
        {
            try
            {
                List<AircraftModel> aircraftModelList = _aircraftModelRepository.List();
                CreateResponse(aircraftModelList, HttpStatusCode.OK, "");

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
                List<AircraftModelDataVM> aircraftModelList = _aircraftModelRepository.List(datatableParams);
                CreateResponse(aircraftModelList, HttpStatusCode.OK, "");

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
                List<DropDownValues> aircraftMakeList = _aircraftModelRepository.ListDropDownValues();
                CreateResponse(aircraftMakeList, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id)
        {
            try
            {
                bool isAlreadyUsed = _aircraftModelRepository.IsAlreadyUsed(id);

                if (isAlreadyUsed)
                {
                    CreateResponse(true, HttpStatusCode.BadRequest, "Aircraft model can't be deleted because it is used in aircraft details!");
                    return _currentResponse;
                }

                _aircraftModelRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft model deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(AircraftModel aircraftModel)
        {
            try
            {
                bool isAircraftModelExist = IsAircraftModelExist(aircraftModel);

                if (isAircraftModelExist)
                {
                    CreateResponse(aircraftModel, HttpStatusCode.Ambiguous, "Aircraft model is already exist");
                }
                else
                {
                    aircraftModel = _aircraftModelRepository.Edit(aircraftModel);
                    CreateResponse(aircraftModel, HttpStatusCode.OK, "Aircraft model updated successfully");
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
