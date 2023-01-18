using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.Aircraft;
using DataModels.VM.User;
using DataModels.Constants;
using DataModels.Enums;
using DataModels.VM.AircraftEquipment;

namespace Service
{
    public class AircraftService : BaseService, IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAircraftCategoryRepository _aircraftCategoryRepository;
        private readonly IAircraftClassRepository _aircraftClassRepository;
        private readonly IAircraftMakeRepository _aircraftMakeRepository;
        private readonly IAircraftModelRepository _aircraftModelRepository;
        private readonly IAircraftEquipmentTimeRepository _aircraftEquipmentTimeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAircraftStatusRepository _aircraftStatusRepository;
        private readonly IAircraftEquipementTimeService _aircraftEquipementTimeService;
        private readonly IUserRepository _userRepository;
        private readonly IAircraftEquipmentRepository _aircraftEquipmentRepository;

        public AircraftService(IAircraftRepository aircraftRepository, IAircraftCategoryRepository aircraftCategoryRepository,
                               IAircraftClassRepository aircraftClassRepository, IAircraftMakeRepository aircraftMakeRepository,
                               IAircraftModelRepository aircraftModelRepository, IAircraftEquipmentTimeRepository aircraftEquipmentTimeRepository,
                               ICompanyRepository companyRepository, IAircraftStatusRepository aircraftStatusRepository,
                               IAircraftEquipementTimeService aircraftEquipementTimeService, IUserRepository userRepository,
                               IAircraftEquipmentRepository aircraftEquipmentRepository)
        {
            _aircraftRepository = aircraftRepository;
            _aircraftCategoryRepository = aircraftCategoryRepository;
            _aircraftClassRepository = aircraftClassRepository;
            _aircraftMakeRepository = aircraftMakeRepository;
            _aircraftModelRepository = aircraftModelRepository;
            _aircraftEquipmentTimeRepository = aircraftEquipmentTimeRepository;
            _companyRepository = companyRepository;
            _aircraftStatusRepository = aircraftStatusRepository;
            _aircraftEquipementTimeService = aircraftEquipementTimeService;
            _userRepository = userRepository;
            _aircraftEquipmentRepository = aircraftEquipmentRepository;
        }

        public CurrentResponse Create(AircraftVM airCraftVM)
        {
            Aircraft airCraft = ToDataObject(airCraftVM);

            try
            {
                //bool isAircraftExist = IsAircraftExist(airCraftVM);

                //if (isAircraftExist)
                //{
                //    CreateResponse(aircraft, HttpStatusCode.Ambiguous, "Aircraft is already exist");
                //}
                //else
                //{
                airCraft = _aircraftRepository.Create(airCraft);
                CreateResponse(airCraft, HttpStatusCode.OK, "Aircraft added successfully.");
                //   }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        //private bool IsAircraftExist(AircraftVM aircraftVM)
        //{
        //    Aircraft aircraft = _aircraftRepository.FindByCondition(p => p.TailNo == airCraftVM.TailNo && p.Id != airCraftVM.Id);

        //    if (aircraft == null)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        public CurrentResponse IsAircraftExist(long id, string tailNo)
        {
            try
            {
                Aircraft airCraft = _aircraftRepository.FindByCondition(p => p.TailNo == tailNo && p.Id != id && p.IsDeleted == false);

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

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _aircraftRepository.Delete(id, deletedBy);
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
                airCraft = _aircraftRepository.Edit(airCraft);
                CreateResponse(airCraft, HttpStatusCode.OK, "Aircraft updated successfully.");
                //}

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(long id, int companyId)
        {
            try
            {
                Aircraft airCraft = _aircraftRepository.FindByCondition(p => p.Id == id && (companyId == 0 ? true : p.CompanyId == companyId));
                AircraftVM aircraftVM = new AircraftVM();

                if (airCraft != null)
                {
                    aircraftVM = ToBusinessObject(airCraft);
                    aircraftVM.AircraftStatus = _aircraftStatusRepository.FindByCondition(p=>p.Id == airCraft.AircraftStatusId);
                }

                aircraftVM.AircraftCategoryList = _aircraftCategoryRepository.ListDropDownValues();
                aircraftVM.AircraftClassList = _aircraftClassRepository.ListDropDownValues();
                aircraftVM.AircraftMakeList = _aircraftMakeRepository.ListDropDownValues(companyId);
                aircraftVM.AircraftModelList = _aircraftModelRepository.ListDropDownValues(companyId);
                aircraftVM.FlightSimulatorClassList = _aircraftRepository.ListFlightSimulatorClassDropDownValues();
                aircraftVM.AircraftStatusList = _aircraftStatusRepository.ListDropDownValues();
                aircraftVM.OwnersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);

                var data  = _aircraftEquipmentTimeRepository.FindListByCondition(p => p.AircraftId == id && p.IsDeleted == false);
                aircraftVM.AircraftEquipmentTimesList = _aircraftEquipementTimeService.ToCreateBusinessObjectList(data);

                if (companyId > 0)
                {
                    aircraftVM.CompanyId = companyId;
                    aircraftVM.CompanyName = _companyRepository.FindByCondition(p => p.Id == companyId).Name;
                }
                else
                {
                    aircraftVM.Companies = _companyRepository.ListDropDownValues();
                }

                aircraftVM.AircraftEquipmentList = GetAircraftEquipmentLists(aircraftVM.Id);

                CreateResponse(aircraftVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch(Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListAircraftDropdownValues(int companyId)
        {
            try
            {
                List<DropDownLargeValues> aircraftList = _aircraftRepository.ListDropDownValues(companyId);
                CreateResponse(aircraftList, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
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

                //aircraftFilterVM.Companies = _companyRepository.ListDropDownValues();

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
                List<AircraftDataVM> airCraftList = _aircraftRepository.List(datatableParams);

                foreach (AircraftDataVM airCraftVM in airCraftList)
                {
                    airCraftVM.ImagePath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.AircraftImage}/{airCraftVM.CompanyId}/{airCraftVM.ImageName}";
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
                List<Aircraft> airCraftList = _aircraftRepository.ListAllByCompanyId(companyId);

                foreach (Aircraft airCraftVM in airCraftList)
                {
                    airCraftVM.ImageName = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}{UploadDirectories.AircraftImage}/{airCraftVM.CompanyId}/{airCraftVM.ImageName}";
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

        public CurrentResponse UpdateImageName(long id, string imageName)
        {
            try
            {
                bool isImageNameUpdated = _aircraftRepository.UpdateImageName(id, imageName);

                CreateResponse(isImageNameUpdated, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateStatus(long id, byte statusId)
        {
            try
            {
                bool isStatusUpdated =  _aircraftRepository.UpdateStatus(id, statusId);

                CreateResponse(isStatusUpdated, HttpStatusCode.OK, "Aircraft status updated successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private List<AircraftEquipmentDataVM> GetAircraftEquipmentLists(long aircraftId)
        {
            AircraftEquipmentDatatableParams datatableParams = new ();

            datatableParams.Start = 1;
            datatableParams.Length = 10;
            datatableParams.SortOrderColumn = "Item";
            datatableParams.OrderType = "ASC";

            datatableParams.AircraftId = aircraftId;

            List<AircraftEquipmentDataVM> aircraftEquipmentDataVMs =  _aircraftEquipmentRepository.List(datatableParams);

            return aircraftEquipmentDataVMs;
        }

        public CurrentResponse LockAircraft(long id, bool isLock)
        {
            try
            {
                bool isLockedForUpdate = _aircraftRepository.UpdateIsLockedForUpdate(id, isLock);

                if (isLockedForUpdate)
                {
                    CreateResponse(isLockedForUpdate, HttpStatusCode.OK, "Aircraft has been locked.");
                }
                else
                {
                    CreateResponse(isLockedForUpdate, HttpStatusCode.OK, "Aircraft has been unlocked");
                }

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }


        private AircraftVM ToBusinessObject(Aircraft airCraft)
        {
            AircraftVM airCraftVM = new AircraftVM();

            airCraftVM.Id = airCraft.Id;
            airCraftVM.TailNo = airCraft.TailNo;
            airCraftVM.ImageName = airCraft.ImageName;
            airCraftVM.ImagePath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}{UploadDirectories.AircraftImage}/{airCraft.CompanyId}/{airCraftVM.ImageName}";

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
            airCraftVM.AircraftStatusId = airCraft.AircraftStatusId;
            airCraftVM.OwnerId = airCraft.OwnerId;
            airCraftVM.IsLock = airCraft.IsLockedForUpdate;

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
            airCraft.AircraftStatusId = airCraftVM.AircraftStatusId;
            airCraft.OwnerId = airCraftVM.OwnerId;
            airCraft.IsLockedForUpdate = airCraftVM.IsLock;

            if (airCraftVM.AircraftStatusId == 0)
            {
                airCraft.AircraftStatusId = (int)AircraftStatuses.ReadyForFlight;
            }

           
            if (airCraftVM.Id == 0)
            {
                airCraft.CreatedOn = DateTime.UtcNow;
                airCraft.CreatedBy = airCraftVM.CreatedBy;
            }
            else
            {
                airCraft.UpdatedOn = DateTime.UtcNow;
                airCraft.UpdatedBy = airCraftVM.UpdatedBy;
            }

            return airCraft;
        }
    }
}
