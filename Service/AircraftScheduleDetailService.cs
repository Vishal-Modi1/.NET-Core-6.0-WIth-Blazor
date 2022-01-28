using DataModels.Entities;
using DataModels.VM;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class AircraftScheduleDetailService : BaseService, IAircraftScheduleDetailService
    {
        private readonly IAircraftScheduleDetailRepository _aircraftScheduleDetailRepository;

        public AircraftScheduleDetailService(IAircraftScheduleDetailRepository aircraftScheduleDetailRepository, IUserRepository userRepository,
            IAircraftRepository aircraftRepository)
        {
            _aircraftScheduleDetailRepository = aircraftScheduleDetailRepository;
        }

        public CurrentResponse Create(AircraftScheduleDetailVM aircraftScheduleDetailVM)
        {
            AircraftScheduleDetail aircraftScheduleDetail = new AircraftScheduleDetail();

            aircraftScheduleDetail.AircraftScheduleId = aircraftScheduleDetailVM.AircraftScheduleId;
            aircraftScheduleDetail.CheckOutBy = aircraftScheduleDetailVM.CheckOutBy;
            aircraftScheduleDetail.CheckOutTime = DateTime.UtcNow;
            aircraftScheduleDetail.FlightStatus = "CheckOut";
            aircraftScheduleDetail.IsCheckOut = true;

            try
            {
                aircraftScheduleDetail = _aircraftScheduleDetailRepository.Create(aircraftScheduleDetail);
                CreateResponse(aircraftScheduleDetail, HttpStatusCode.OK, "Aircraft check out successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private AircraftScheduleDetail ToDataObject(AircraftScheduleDetailVM aircraftScheduleDetailVM)
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
