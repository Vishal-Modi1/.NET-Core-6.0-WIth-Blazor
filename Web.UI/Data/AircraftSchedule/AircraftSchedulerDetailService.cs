using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.AircraftSchedule
{
    public class AircraftSchedulerDetailService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftSchedulerDetailService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> CheckOut(DependecyParams dependecyParams, long scheduleId)
        {
            string jsonData = JsonConvert.SerializeObject(scheduleId);

            dependecyParams.URL  = "aircraftschedulerdetail/checkout?scheduleId="+scheduleId;

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UnCheckOut(DependecyParams dependecyParams, long scheduleId)
        {
            dependecyParams.URL  = "aircraftschedulerdetail/uncheckout?scheduleId=" + scheduleId;

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> CheckIn(DependecyParams dependecyParams, List<AircraftEquipmentTimeVM> aircraftEquipmentsTimeList)
        {
            AircraftEquipmentTimeRequestVM aircraftEquipmentTimeRequestVM = new AircraftEquipmentTimeRequestVM();
            aircraftEquipmentTimeRequestVM.Data = aircraftEquipmentsTimeList;

            dependecyParams.JsonData = JsonConvert.SerializeObject(aircraftEquipmentTimeRequestVM);

            dependecyParams.URL  = "aircraftschedulerdetail/checkin";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }


        public async Task<CurrentResponse> IsAircraftAlreadyCheckOutAsync(DependecyParams dependecyParams, long aircraftId)
        {
            dependecyParams.URL  = $"aircraftschedulerdetail/isaircraftalreadycheckout?aircraftId={aircraftId}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
