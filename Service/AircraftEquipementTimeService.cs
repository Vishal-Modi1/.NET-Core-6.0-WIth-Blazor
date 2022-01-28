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

        public CurrentResponse Create(AircraftEquipmentTimeVM aircraftEquipmentTimeVM)
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

        public CurrentResponse Delete(int id)
        {
            try
            {
                _aircraftEquipementTimeRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Aircraft Equipment Time is deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public AircraftEquipmentTime ToDataObject(AircraftEquipmentTimeVM aircraftEquipmentTimeVM)
        {
            AircraftEquipmentTime aircraftEquipmentTime = new AircraftEquipmentTime();

            aircraftEquipmentTime.Id = aircraftEquipmentTimeVM.Id;
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

        public bool DeleteAllEquipmentTimeByAirCraftId(long AirCraftId,int UpdatedBy)
        {
            try
            {
                _aircraftEquipementTimeRepository.DeleteEquipmentTimes(AirCraftId, UpdatedBy);
                return true;
            }
            catch (Exception exc)
            {
                return false;   
            }
        }
    }
}
