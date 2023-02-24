using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.LogBook
{
    public class LogBookService
    {
        private readonly HttpCaller _httpCaller;

        public LogBookService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<LogBookDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            try
            {
                dependecyParams.URL = "logBook/list";
                dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
                CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

                List<LogBookDataVM> discrepanciesList = JsonConvert.DeserializeObject<List<LogBookDataVM>>(response.Data.ToString());

                return discrepanciesList;
            }
            catch (Exception exc)
            {
                return new List<LogBookDataVM>();
            }
        }

        public async Task<List<DropDownSmallValues>> ListPassengersRolesDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"logBook/listPassengersRolesDropdownValues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownSmallValues> passengersRoles = new List<DropDownSmallValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                passengersRoles = JsonConvert.DeserializeObject<List<DropDownSmallValues>>(response.Data.ToString());
            }

            return passengersRoles;
        }

        public async Task<List<DropDownLargeValues>> ListPassengersDropdownValuesByCompanyId(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"logBook/listPassengersDropdownValuesByCompanyId";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownLargeValues> passengersRoles = new List<DropDownLargeValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                passengersRoles = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return passengersRoles;
        }

        public async Task<List<DropDownSmallValues>> ListInstrumentApproachesDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"logBook/listInstrumentApproachesDropdownValues";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<DropDownSmallValues> instrumentApproaches = new List<DropDownSmallValues>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                instrumentApproaches = JsonConvert.DeserializeObject<List<DropDownSmallValues>>(response.Data.ToString());
            }

            return instrumentApproaches;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, LogBookVM logBookVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(logBookVM);
            dependecyParams.URL = "logBook/create";

            if (logBookVM.Id > 0)
            {
                dependecyParams.URL = "logBook/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> SaveandUpdateCrewPassengerAsync(DependecyParams dependecyParams, CrewPassengerVM crewPassengerVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(crewPassengerVM);
            dependecyParams.URL = "logBook/createCrewPassenger";

            if (crewPassengerVM.Id > 0)
            {
                dependecyParams.URL = "logBook/createCrewPassenger";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UploadFlightPhotosAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"logBook/uploadFlightPhotos";

            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }

        public async Task<LogBookFilterVM> GetFiltersAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"logBook/getfilters";

            var response = await _httpCaller.GetAsync(dependecyParams);

            LogBookFilterVM logBookFilterVM = new LogBookFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                logBookFilterVM = JsonConvert.DeserializeObject<LogBookFilterVM>(response.Data.ToString());
            }

            return logBookFilterVM;
        }

        public async Task<List<LogBookSummaryVM>> LogBookSummaries(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"logBook/logBookSummaries";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            List<LogBookSummaryVM> logBookSummaries = new List<LogBookSummaryVM>();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                logBookSummaries = JsonConvert.DeserializeObject<List<LogBookSummaryVM>>(response.Data.ToString());
            }

            return logBookSummaries;
        }

        public async Task<LogBookVM> GetDetails(DependecyParams dependecyParams,long id)
        {
            dependecyParams.URL = $"logBook/getDetails?id={id}";

            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
            LogBookVM logBookVM = new LogBookVM();

            if (response != null && response.Data != null && response.Status == System.Net.HttpStatusCode.OK)
            {
                logBookVM = JsonConvert.DeserializeObject<LogBookVM>(response.Data.ToString());
            }

            return logBookVM;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"logBook/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeletePhotoAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"logBook/deletePhoto?photoId={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteLogBookInstrumentApproachAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"logBook/deleteLogBookInstrumentApproach?instrumentApproachId={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }


        public async Task<CurrentResponse> DeleteLogBookCrewPassengerAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"logBook/deleteLogBookCrewPassenger?logBookCrewPassengerId={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }
    }
}
