using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.Aircraft;
using DataModels.VM.User;

namespace Service
{
    public class AircraftService : BaseService, IAircraftService
    {
        private readonly IAircraftRepository _airCraftRepository;
        private readonly IAircraftCategoryRepository _aircraftCategoryRepository;
        private readonly IAircraftClassRepository _aircraftClassRepository;
        private readonly IAircraftMakeRepository _aircraftMakeRepository;
        private readonly IAircraftModelRepository _aircraftModelRepository;
        private readonly IAircraftEquipmentTimeRepository _aircraftEquipmentTimeRepository;
        private readonly ICompanyRepository _companyRepository;

        public AircraftService(IAircraftRepository airCraftRepository, IAircraftCategoryRepository aircraftCategoryRepository,
                               IAircraftClassRepository aircraftClassRepository, IAircraftMakeRepository aircraftMakeRepository,
                               IAircraftModelRepository aircraftModelRepository, IAircraftEquipmentTimeRepository aircraftEquipmentTimeRepository,
                                ICompanyRepository companyRepository)
        {
            _airCraftRepository = airCraftRepository;
            _aircraftCategoryRepository = aircraftCategoryRepository;
            _aircraftClassRepository = aircraftClassRepository;
            _aircraftMakeRepository = aircraftMakeRepository;
            _aircraftModelRepository = aircraftModelRepository;
            _aircraftEquipmentTimeRepository = aircraftEquipmentTimeRepository;
            _companyRepository = companyRepository; 
        }

        public CurrentResponse Create(AircraftVM airCraftVM)
        {
            Aircraft airCraft = ToDataObject(airCraftVM);

            try
            {
                //bool isAirCraftExist = IsAirCraftExist(airCraftVM);

                //if (isAirCraftExist)
                //{
                //    CreateResponse(airCraft, HttpStatusCode.Ambiguous, "Aircraft is already exist");
                //}
                //else
                //{
                airCraft = _airCraftRepository.Create(airCraft);
                CreateResponse(airCraft, HttpStatusCode.OK, "Aircraft added successfully");
                //   }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        //private bool IsAirCraftExist(AirCraftVM airCraftVM)
        //{
        //    AirCraft airCraft = _airCraftRepository.FindByCondition(p => p.TailNo == airCraftVM.TailNo && p.Id != airCraftVM.Id);

        //    if (airCraft == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public CurrentResponse IsAirCraftExist(int id, string tailNo)
        {
            try
            {
                Aircraft airCraft = _airCraftRepository.FindByCondition(p => p.TailNo == tailNo && p.Id != id);

                if (airCraft != null)
                {
                    CreateResponse(true, HttpStatusCode.OK, "Aircraft is already exist");
                }
                else
                {
                    CreateResponse(false, HttpStatusCode.OK, "");
                }

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id)
        {
            try
            {
                _airCraftRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft is deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(AircraftVM airCraftVM)
        {
            Aircraft airCraft = ToDataObject(airCraftVM);

            try
            {
                //bool isInstructorTypeExist = IsAirCraftExist(airCraftVM);

                //if (isInstructorTypeExist)
                //{
                //    CreateResponse(airCraft, HttpStatusCode.Ambiguous, "Aircraft is already exist");
                //}

                //else
                //{
                airCraft = _airCraftRepository.Edit(airCraft);
                CreateResponse(airCraft, HttpStatusCode.OK, "Aircraft updated successfully");
                //}

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(int id, int companyId)
        {
            Aircraft airCraft = _airCraftRepository.FindByCondition(p => p.Id == id && (companyId == 0 ? true : p.CompanyId == companyId));
            AircraftVM airCraftVM = new AircraftVM();

            if (airCraft != null)
            {
                airCraftVM = ToBusinessObject(airCraft);
            }

            airCraftVM.AircraftCategoryList = _aircraftCategoryRepository.ListDropDownValues();
            airCraftVM.AircraftClassList = _aircraftClassRepository.ListDropDownValues();
            airCraftVM.AircraftMakeList = _aircraftMakeRepository.ListDropDownValues();
            airCraftVM.AircraftModelList = _aircraftModelRepository.ListDropDownValues();
            airCraftVM.AircraftEquipmentTimesList = _aircraftEquipmentTimeRepository.FindListByCondition(p => p.AircraftId == id);
            airCraftVM.FlightSimulatorClassList = _airCraftRepository.ListFlightSimulatorClassDropDownValues();

            if (companyId > 0)
            {
                airCraftVM.CompanyId = companyId;
                airCraftVM.CompanyName = _companyRepository.FindByCondition(p => p.Id == companyId).Name;
            }
            else
            {
                airCraftVM.Companies = _companyRepository.ListDropDownValues();
            }

            CreateResponse(airCraftVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }

        private AircraftVM ToBusinessObject(Aircraft airCraft)
        {
            AircraftVM airCraftVM = new AircraftVM();

            airCraftVM.Id = airCraft.Id;
            airCraftVM.TailNo = airCraft.TailNo;
            airCraftVM.ImageName = airCraft.ImageName;
            airCraftVM.ImagePath = $"{Configuration.ConfigurationSettings.Instance.AircraftImagePathPrefix}{airCraftVM.ImageName}";

            airCraftVM.Year = airCraft.Year;
            airCraftVM.AircraftMakeId = airCraft.AircraftMakeId;
            airCraftVM.AircraftModelId = airCraft.AircraftModelId;
            airCraftVM.AircraftCategoryId = airCraft.AircraftCategoryId;
            airCraftVM.AircraftClassId = airCraft.AircraftClassId;
            airCraftVM.FlightSimulatorClassId = airCraft.FlightSimulatorClassId;
            airCraftVM.NoofEngines = airCraft.NoofEngines;
            airCraftVM.NoofPropellers = airCraft.NoofPropellers;
            airCraftVM.IsEngineshavePropellers = airCraft.IsEngineshavePropellers;
            airCraftVM.IsEnginesareTurbines = airCraft.IsEnginesareTurbines;
            airCraftVM.TrackOilandFuel = airCraft.TrackOilandFuel;
            airCraftVM.IsIdentifyMeterMismatch = airCraft.IsIdentifyMeterMismatch;
            airCraftVM.CompanyId = airCraft.CompanyId;

            airCraftVM.IsActive = airCraft.IsActive;
            airCraftVM.IsDeleted = airCraft.IsDeleted;
            airCraftVM.CreatedBy = airCraft.CreatedBy;
            airCraftVM.UpdatedBy = airCraft.UpdatedBy;
            airCraftVM.DeletedBy = airCraft.DeletedBy;
            airCraftVM.CreatedOn = airCraft.CreatedOn;
            airCraftVM.UpdatedOn = airCraft.UpdatedOn;
            airCraftVM.DeletedOn = airCraft.DeletedOn;

            return airCraftVM;
        }

        public List<AircraftVM> ToBusinessObjectList(List<Aircraft> aircraftList)
        {
            List<AircraftVM> airCraftVMList = new List<AircraftVM>();

            foreach (Aircraft aircraft in aircraftList)
            {
                AircraftVM airCraftVM = ToBusinessObject(aircraft);

                airCraftVMList.Add(airCraftVM);
            }

            return airCraftVMList;
        }

        public Aircraft ToDataObject(AircraftVM airCraftVM)
        {
            Aircraft airCraft = new Aircraft();

            airCraft.Id = airCraftVM.Id;
            airCraft.TailNo = airCraftVM.TailNo;

            airCraft.Year = airCraftVM.Year;
            airCraft.AircraftMakeId = airCraftVM.AircraftMakeId;
            airCraft.AircraftModelId = airCraftVM.AircraftModelId;
            airCraft.AircraftCategoryId = airCraftVM.AircraftCategoryId;
            airCraft.AircraftClassId = airCraftVM.AircraftClassId == 0 ? null : airCraftVM.AircraftClassId;
            airCraft.FlightSimulatorClassId = airCraftVM.FlightSimulatorClassId == 0 ? null : airCraftVM.FlightSimulatorClassId;
            airCraft.NoofEngines = airCraftVM.NoofEngines;
            airCraft.NoofPropellers = airCraftVM.NoofPropellers;
            airCraft.IsEngineshavePropellers = airCraftVM.IsEngineshavePropellers;
            airCraft.IsEnginesareTurbines = airCraftVM.IsEnginesareTurbines;
            airCraft.TrackOilandFuel = airCraftVM.TrackOilandFuel;
            airCraft.IsIdentifyMeterMismatch = airCraftVM.IsIdentifyMeterMismatch;
            airCraft.IsActive = true;
            airCraft.CompanyId = airCraftVM.CompanyId;

            airCraft.CreatedBy = airCraftVM.CreatedBy;
            airCraft.UpdatedBy = airCraftVM.UpdatedBy;

            if (airCraftVM.Id == 0)
            {
                airCraft.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                airCraft.UpdatedOn = DateTime.UtcNow;
            }

            return airCraft;
        }

        public CurrentResponse GetFiltersValue(int companyId)
        {
            try
            {
                AircraftFilterVM aircraftFilterVM = new AircraftFilterVM();


                if (companyId > 0)
                {
                    aircraftFilterVM.CompanyId = companyId;
                    aircraftFilterVM.CompanyName = _companyRepository.FindByCondition(p => p.Id == companyId).Name;
                }
                else
                {
                    aircraftFilterVM.Companies = _companyRepository.ListDropDownValues();
                }

                aircraftFilterVM.Companies = _companyRepository.ListDropDownValues();

                CreateResponse(aircraftFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new UserFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(AircraftDatatableParams datatableParams)
        {
            try
            {
                List<AircraftDataVM> airCraftList = _airCraftRepository.List(datatableParams);

                foreach (AircraftDataVM airCraftVM in airCraftList)
                {
                    airCraftVM.ImagePath = $"{Configuration.ConfigurationSettings.Instance.AircraftImagePathPrefix}{airCraftVM.ImageName}";
                }

                CreateResponse(airCraftList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListAllByCompanyId(int companyId)
        {
            try
            {
                List<Aircraft> airCraftList = _airCraftRepository.ListAllByCompanyId(companyId);

                foreach (Aircraft airCraftVM in airCraftList)
                {
                    airCraftVM.ImageName = $"{Configuration.ConfigurationSettings.Instance.AircraftImagePathPrefix}{airCraftVM.ImageName}";
                }

                CreateResponse(airCraftList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateImageName(int id, string imageName)
        {
            try
            {
                bool isImageNameUpdated = _airCraftRepository.UpdateImageName(id, imageName);

                CreateResponse(isImageNameUpdated, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
