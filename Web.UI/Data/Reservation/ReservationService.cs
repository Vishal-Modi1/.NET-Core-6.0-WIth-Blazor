using DataModels.VM.Common;
using DataModels.VM.Reservation;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.Reservation
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

        public async Task<List<UpcomingFlight>> ListUpcomingFlightsByUserId(DependecyParams dependecyParams, long userId)
        {
            dependecyParams.URL = $"reservation/listUpcomingFlightsByUserId?userId={userId}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<UpcomingFlight> list = UpcomingFlightsList(response);

            return list;
        }

        public async Task<List<UpcomingFlight>> ListUpcomingFlightsByAircraftId(DependecyParams dependecyParams, long aircraftId)
        {
            dependecyParams.URL = $"reservation/listUpcomingFlightsByAircraftId?aircraftId={aircraftId}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<UpcomingFlight> list = UpcomingFlightsList(response);

            return list;
        }

        public async Task<List<UpcomingFlight>> ListUpcomingFlightsByCompanyId(DependecyParams dependecyParams, int companyId)
        {
            dependecyParams.URL = $"reservation/listUpcomingFlightsByCompanyId?companyId={companyId}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            List<UpcomingFlight> list = UpcomingFlightsList(response);

            return list;
        }

        private List<UpcomingFlight> UpcomingFlightsList(CurrentResponse response)
        {
            List<UpcomingFlight> list = new List<UpcomingFlight>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                list = JsonConvert.DeserializeObject<List<UpcomingFlight>>(response.Data.ToString());
            }

            return list;
        }
    }
}
