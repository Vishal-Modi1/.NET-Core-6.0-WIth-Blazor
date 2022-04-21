using DataModels.VM.Reservation;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Reservation
{
    public class ReservationService
    {
        private readonly HttpCaller _httpCaller;

        public ReservationService(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(navigationManager, authenticationStateProvider);
        }

        public async Task<List<ReservationDataVM>> ListAsync(IHttpClientFactory httpClient, DatatableParams datatableParams)
        {
            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync( httpClient, "reservation/List", jsonData);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<ReservationDataVM>();
            }

            List<ReservationDataVM> reservationsList = JsonConvert.DeserializeObject<List<ReservationDataVM>>(response.Data.ToString());

            return reservationsList; 
        }

        public async Task<ReservationFilterVM> GetFiltersAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"reservation/getfilters");

            ReservationFilterVM reservationFilterVM = new ReservationFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                reservationFilterVM = JsonConvert.DeserializeObject<ReservationFilterVM>(response.Data.ToString());
            }

            return reservationFilterVM;
        }
    }
}
