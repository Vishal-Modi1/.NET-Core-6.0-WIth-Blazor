using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Service
{
    public class AircraftEquipementTimeService : BaseService, IAircraftEquipementTimeService
    {
        private readonly IAircraftEquipmentTimeRepository _aircraftEquipementTimeRepository;

        public AircraftEquipementTimeService(IAircraftEquipmentTimeRepository aircraftEquipementTimeRepository)
        {
            _aircraftEquipementTimeRepository = aircraftEquipementTimeRepository;
        }

        public CurrentResponse Create(AircraftEquipmentTimeCreateVM aircraftEquipmentTimeVM)
        {
            AircraftEquipmentTime aircraftEquipmentTime = ToDataObject(aircraftEquipmentTimeVM);
            try
            {
                aircraftEquipmentTime = _aircraftEquipementTimeRepository.Create(aircraftEquipmentTime);
                CreateResponse(aircraftEquipmentTimeVM, HttpStatusCode.OK, "Aircraft Equipment Time added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(AircraftEquipmentTimeVM aircraftEquipmentTimeVM)
        {
            AircraftEquipmentTime aircraftEquipmentTime = ToDataObject(aircraftEquipmentTimeVM);

            try
            {
                aircraftEquipmentTime = _aircraftEquipementTimeRepository.Edit(aircraftEquipmentTime);
                CreateResponse(aircraftEquipmentTime, HttpStatusCode.OK, "Aircraft Equipment Time updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id, long deletedBy)
        {
            try
            {
                _aircraftEquipementTimeRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft Equipment Time is deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public AircraftEquipmentTime ToDataObject(AircraftEquipmentTimeVM aircraftEquipmentTime)
        {
            AircraftEquipmentTime aircraftEquipmentTimeVM = new AircraftEquipmentTime();

            aircraftEquipmentTimeVM.Id = aircraftEquipmentTime.Id;
            aircraftEquipmentTimeVM.EquipmentName = aircraftEquipmentTime.EquipmentName;
            aircraftEquipmentTimeVM.Hours = aircraftEquipmentTime.Hours;
            aircraftEquipmentTimeVM.AircraftId = aircraftEquipmentTime.AircraftId;

            aircraftEquipmentTimeVM.CreatedBy = aircraftEquipmentTime.CreatedBy;
            aircraftEquipmentTimeVM.CreatedOn = aircraftEquipmentTime.CreatedOn;
            aircraftEquipmentTimeVM.UpdatedBy = aircraftEquipmentTime.UpdatedBy;
            aircraftEquipmentTimeVM.UpdatedOn = aircraftEquipmentTime.UpdatedOn;

            return aircraftEquipmentTimeVM;
        }

        //public List<AircraftEquipmentTimeVM> ToBusinessObjectList(List<AircraftEquipmentTime> aircraftEquipmentTimesList)
        //{
        //    List<AircraftEquipmentTimeVM> aircraftEquipmentTimeVMList = new List<AircraftEquipmentTimeVM>();

        //    foreach (AircraftEquipmentTime item in aircraftEquipmentTimesList)
        //    {
        //        AircraftEquipmentTimeVM aircraftEquipmentTimeVM = new AircraftEquipmentTimeVM();


        //    }

        //    return aircraftEquipmentTimeVMList;
        //}

        public AircraftEquipmentTimeVM ToBusinessObject(AircraftEquipmentTime aircraftEquipmentTime)
        {
            AircraftEquipmentTimeVM aircraftEquipmentTimeVM = new AircraftEquipmentTimeVM();

            aircraftEquipmentTimeVM.Id = aircraftEquipmentTimeVM.Id;
            aircraftEquipmentTimeVM.EquipmentName = aircraftEquipmentTimeVM.EquipmentName;
            aircraftEquipmentTimeVM.Hours = aircraftEquipmentTimeVM.Hours;
            aircraftEquipmentTimeVM.AircraftId = aircraftEquipmentTimeVM.AircraftId;

            aircraftEquipmentTimeVM.CreatedBy = aircraftEquipmentTimeVM.CreatedBy;

            if (aircraftEquipmentTimeVM.Id == 0)
            {
                aircraftEquipmentTimeVM.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                aircraftEquipmentTimeVM.UpdatedBy = aircraftEquipmentTimeVM.UpdatedBy;
                aircraftEquipmentTimeVM.UpdatedOn = DateTime.UtcNow;
            }

            return aircraftEquipmentTimeVM;
        }

        public AircraftEquipmentTime ToDataObject(AircraftEquipmentTimeCreateVM aircraftEquipmentTimeVM)
        {
            AircraftEquipmentTime aircraftEquipmentTime = new AircraftEquipmentTime();

            aircraftEquipmentTime.Id = 0;
            aircraftEquipmentTime.EquipmentName = aircraftEquipmentTimeVM.EquipmentName;
            aircraftEquipmentTime.Hours = aircraftEquipmentTimeVM.Hours;
            aircraftEquipmentTime.AircraftId = aircraftEquipmentTimeVM.AircraftId;

            aircraftEquipmentTime.CreatedBy = aircraftEquipmentTimeVM.CreatedBy;

            if (aircraftEquipmentTimeVM.Id == 0)
            {
                aircraftEquipmentTime.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                aircraftEquipmentTime.UpdatedBy = aircraftEquipmentTimeVM.UpdatedBy;
                aircraftEquipmentTime.UpdatedOn = DateTime.UtcNow;
            }

            return aircraftEquipmentTime;
        }

        public AircraftEquipmentTimeCreateVM ToCreateBusinessObject(AircraftEquipmentTime aircraftEquipmentTime)
        {
            AircraftEquipmentTimeCreateVM aircraftEquipmentTimeVM = new AircraftEquipmentTimeCreateVM();

            aircraftEquipmentTimeVM.Id = aircraftEquipmentTime.Id;
            aircraftEquipmentTimeVM.EquipmentName = aircraftEquipmentTime.EquipmentName;
            aircraftEquipmentTimeVM.Hours = aircraftEquipmentTime.Hours;
            aircraftEquipmentTimeVM.AircraftId = aircraftEquipmentTime.AircraftId;

            aircraftEquipmentTimeVM.CreatedBy = aircraftEquipmentTime.CreatedBy;
            aircraftEquipmentTimeVM.CreatedOn = aircraftEquipmentTime.CreatedOn;
            aircraftEquipmentTimeVM.UpdatedBy = aircraftEquipmentTimeVM.UpdatedBy;
            aircraftEquipmentTimeVM.UpdatedOn = aircraftEquipmentTime.UpdatedOn;

            return aircraftEquipmentTimeVM;
        }

        public List<AircraftEquipmentTimeCreateVM> ToCreateBusinessObjectList(List<AircraftEquipmentTime> aircraftEquipmentTimesList)
        {
            List<AircraftEquipmentTimeCreateVM> aircraftEquipmentTimeCreateVMList = new List<AircraftEquipmentTimeCreateVM>();

            foreach (AircraftEquipmentTime aircraftEquipmentTimesVM in aircraftEquipmentTimesList)
            {
                AircraftEquipmentTimeCreateVM aircraftEquipmentTime = ToCreateBusinessObject(aircraftEquipmentTimesVM);
                aircraftEquipmentTimeCreateVMList.Add(aircraftEquipmentTime);
            }

            return aircraftEquipmentTimeCreateVMList;
        }

        public List<AircraftEquipmentTime> ToDataObjectList(List<AircraftEquipmentTimeVM> aircraftEquipmentTimesVMList)
        {
            List<AircraftEquipmentTime> aircraftEquipmentTimesList = new List<AircraftEquipmentTime>();

            foreach (AircraftEquipmentTimeVM aircraftEquipmentTimesVM in aircraftEquipmentTimesVMList)
            {
                AircraftEquipmentTime aircraftEquipmentTime = ToDataObject(aircraftEquipmentTimesVM);

                aircraftEquipmentTimesList.Add(aircraftEquipmentTime);
            }

            return aircraftEquipmentTimesList;
        }

        public bool DeleteAllEquipmentTimeByAircraftId(long AircraftId, long UpdatedBy)
        {
            try
            {
                _aircraftEquipementTimeRepository.DeleteEquipmentTimes(AircraftId, UpdatedBy);
                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}
