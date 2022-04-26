using DataModels.VM.Common;
using DataModels.VM.Reservation;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Reservation
{
    public class ReservationService
    {
        private readonly HttpCaller _httpCaller;

        public ReservationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<ReservationDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "reservation/List"; 

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<ReservationDataVM>();
            }

            List<ReservationDataVM> reservationsList = JsonConvert.DeserializeObject<List<ReservationDataVM>>(response.Data.ToString());

            return reservationsList; 
        }

        public async Task<ReservationFilterVM> GetFiltersAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"reservation/getfilters";

            var response = await _httpCaller.GetAsync(dependecyParams);

            ReservationFilterVM reservationFilterVM = new ReservationFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                reservationFilterVM = JsonConvert.DeserializeObject<ReservationFilterVM>(response.Data.ToString());
            }

            return reservationFilterVM;
        }
    }
}
