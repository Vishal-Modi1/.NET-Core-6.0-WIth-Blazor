using DataModels.Entities;
using DataModels.VM;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using DataModels.VM.AircraftEquipment;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class AircraftScheduleDetailService : BaseService, IAircraftScheduleDetailService
    {
        private readonly IAircraftScheduleDetailRepository _aircraftScheduleDetailRepository;
        private readonly IAircraftScheduleHobbsTimeRepository _aircraftScheduleHobbsTimeRepository;
        private readonly IAircraftEquipementTimeService _aircraftEquipementTimeService;
        private readonly IAircraftEquipmentTimeRepository _aircraftEquipmentTimeRepository;
        public AircraftScheduleDetailService(IAircraftScheduleDetailRepository aircraftScheduleDetailRepository, 
            IAircraftScheduleHobbsTimeRepository aircraftScheduleHobbsTimeRepository,
            IAircraftEquipementTimeService aircraftEquipementTimeService,
            IAircraftEquipmentTimeRepository aircraftEquipmentTimeRepository)
        {
            _aircraftScheduleDetailRepository = aircraftScheduleDetailRepository;
            _aircraftScheduleHobbsTimeRepository = aircraftScheduleHobbsTimeRepository;
            _aircraftEquipementTimeService = aircraftEquipementTimeService;
            _aircraftEquipmentTimeRepository = aircraftEquipmentTimeRepository;
        }

        public CurrentResponse CheckOut(AircraftScheduleDetailVM aircraftScheduleDetailVM)
        {
            AircraftScheduleDetail aircraftScheduleDetail = new AircraftScheduleDetail();

            aircraftScheduleDetail.AircraftScheduleId = aircraftScheduleDetailVM.AircraftScheduleId;
            aircraftScheduleDetail.CheckOutBy = aircraftScheduleDetailVM.CheckOutBy;
            aircraftScheduleDetail.CheckOutTime = DateTime.UtcNow;
            aircraftScheduleDetail.FlightStatus = "CheckOut";
            aircraftScheduleDetail.IsCheckOut = true;

            try
            {
                aircraftScheduleDetail = _aircraftScheduleDetailRepository.CheckOut(aircraftScheduleDetail);
                CreateResponse(aircraftScheduleDetail, HttpStatusCode.OK, "Aircraft check out successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse CheckIn(List<AircraftEquipmentTimeVM> aircraftEquipmentsTimeList, long checkInBy)
        {
            try
            {
                List<AircraftScheduleHobbsTime> aircraftScheduleHobbsTimesList = new List<AircraftScheduleHobbsTime>();

                foreach (AircraftEquipmentTimeVM aircraftEquipmentTime in aircraftEquipmentsTimeList)
                {
                    AircraftScheduleHobbsTime aircraftScheduleHobbsTime = new AircraftScheduleHobbsTime();

                    aircraftScheduleHobbsTime.AircraftScheduleId = aircraftEquipmentTime.AircraftScheduleId;
                    aircraftScheduleHobbsTime.AircraftEquipmentTimeId = aircraftEquipmentTime.Id;
                    aircraftScheduleHobbsTime.OutTime = aircraftEquipmentTime.Hours;
                    aircraftScheduleHobbsTime.InTime = aircraftEquipmentTime.Hours + aircraftEquipmentTime.TotalHours;
                    aircraftScheduleHobbsTime.TotalTime = aircraftEquipmentTime.TotalHours;

                    aircraftScheduleHobbsTimesList.Add(aircraftScheduleHobbsTime);
                }

                aircraftScheduleHobbsTimesList = _aircraftScheduleHobbsTimeRepository.Create(aircraftScheduleHobbsTimesList);

                List<AircraftEquipmentTime> aircraftEquipmentTimesList = _aircraftEquipementTimeService.ToDataObjectList(aircraftEquipmentsTimeList);

                foreach (AircraftEquipmentTime aircraftEquipmentTime in aircraftEquipmentTimesList)
                {
                    var data = aircraftEquipmentsTimeList.Where(p=> p.Id == aircraftEquipmentTime.Id).First();

                    aircraftEquipmentTime.Hours = data.Hours + data.TotalHours;
                    _aircraftEquipmentTimeRepository.Edit(aircraftEquipmentTime);
                }

                AircraftScheduleDetail aircraftScheduleDetail = _aircraftScheduleDetailRepository.CheckIn(checkInBy, DateTime.UtcNow, aircraftEquipmentsTimeList.First().AircraftScheduleId);
                
                CreateResponse(null, HttpStatusCode.OK, "Aircraft check in successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public AircraftScheduleDetail ToDataObject(AircraftScheduleDetailVM aircraftScheduleDetailVM)
        {
            AircraftScheduleDetail aircraftScheduleDetail = new AircraftScheduleDetail();

            aircraftScheduleDetail.Id = aircraftScheduleDetailVM.Id;
            aircraftScheduleDetail.AircraftScheduleId = aircraftScheduleDetailVM.AircraftScheduleId;
            aircraftScheduleDetail.CheckInTime = aircraftScheduleDetailVM.CheckInTime;
            aircraftScheduleDetail.CheckOutTime = aircraftScheduleDetailVM.CheckOutTime;
            aircraftScheduleDetail.CheckInBy = aircraftScheduleDetailVM.CheckInBy;
            aircraftScheduleDetail.CheckOutBy = aircraftScheduleDetailVM.CheckOutBy;
            aircraftScheduleDetail.CheckInTime = aircraftScheduleDetailVM.CheckInTime;
            aircraftScheduleDetail.IsCheckOut = aircraftScheduleDetailVM.IsCheckOut;

            return aircraftScheduleDetail;
        }

        public CurrentResponse IsAircraftAlreadyCheckOut(long aircraftId)
        {
            try
            {
                bool response = _aircraftScheduleDetailRepository.IsAircraftAlreadyCheckOut(aircraftId);

                if (response)
                {
                    CreateResponse(true, HttpStatusCode.OK, "Aircraft is already checked-out. The aircraft must be checked-in before you can check it out.");
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
    }
}
