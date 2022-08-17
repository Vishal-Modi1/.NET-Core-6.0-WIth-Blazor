using DataModels.VM.Aircraft.AircraftStatusLog;
using DataModels.VM.Common;
using DataModels.VM.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace FSM.Blazor.Data.Aircraft.AircraftStatusLog
{
    public class AircraftStatusLogService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftStatusLogService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<AircraftStatusLogDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "aircraftstatuslog/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<AircraftStatusLogDataVM>();
            }

            List<AircraftStatusLogDataVM> companies = JsonConvert.DeserializeObject<List<AircraftStatusLogDataVM>>(response.Data.ToString());

            return companies;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, CompanyVM companyVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(companyVM);
            dependecyParams.URL = "aircraftstatuslog/create";

            if (companyVM.Id > 0)
            {
                dependecyParams.URL = "aircraftstatuslog/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"aircraftstatuslog/delete?id={id}";

            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"company/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
