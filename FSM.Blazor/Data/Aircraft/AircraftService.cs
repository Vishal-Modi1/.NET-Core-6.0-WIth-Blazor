using DataModels.VM.Aircraft;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Shared;
using DE = DataModels.Entities;
using Microsoft.JSInterop;

namespace FSM.Blazor.Data.Aircraft
{
    public class AircraftService
    {
        private readonly HttpCaller _httpCaller;
        
        [CascadingParameter]
        public Error? Error { get; set; }

        public AircraftService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<AircraftDataVM>> ListAsync(IHttpClientFactory httpClient, AircraftDatatableParams datatableParams)
        {
            try
            {
                string url = "aircraft/list";

                string jsonData = JsonConvert.SerializeObject(datatableParams);

                CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);
                List<AircraftDataVM> aircraftList = JsonConvert.DeserializeObject<List<AircraftDataVM>>(response.Data.ToString());

                return aircraftList;
            }
            catch(Exception exc)
            {
                Error?.ProcessError(exc);
                return null;
            }
        }

        public async Task<List<DE.Aircraft>> ListAllAsync(IHttpClientFactory httpClient)
        {
            try
            {
                string url = "aircraft/listall";

                CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);
                List<DE.Aircraft> aircraftList = JsonConvert.DeserializeObject<List<DE.Aircraft>>(response.Data.ToString());

                return aircraftList;
            }
            catch (Exception exc)
            {
                Error?.ProcessError(exc);
                return null;
            }
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, AircraftVM aircraftVM)
        {
            string jsonData = JsonConvert.SerializeObject(aircraftVM);

            string url = "aircraft/create";

            if (aircraftVM.Id > 0)
            {
                url = "aircraft/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);
           
            return response;
        }

        public async Task<CurrentResponse> SaveandUpdateEquipmentTimeListAsync(IHttpClientFactory httpClient, AircraftVM aircraftVM)
        {
            string url = "aircraft/createaircraftequipment";

            string jsonData = JsonConvert.SerializeObject(aircraftVM.AircraftEquipmentTimesList);
            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            string url = $"aircraft/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

        public async Task<AircraftFilterVM> GetFiltersAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"aircraft/getfilters");

            AircraftFilterVM aircraftFilterVM = new AircraftFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                aircraftFilterVM = JsonConvert.DeserializeObject<AircraftFilterVM>(response.Data.ToString());
            }

            return aircraftFilterVM;
        }

        public async Task<AircraftVM> GetDetailsAsync(IHttpClientFactory httpClient, long id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"aircraft/getDetails?id={id}");

            AircraftVM airCraftVM = new AircraftVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftVM = JsonConvert.DeserializeObject<AircraftVM>(response.Data.ToString());
            }

            return airCraftVM;
        }

        public async Task<List<DropDownLargeValues>> ListDropdownValues(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"aircraft/listdropdownvalues");

            List<DropDownLargeValues> airCraftList = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftList = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return airCraftList;
        }

        public async Task<CurrentResponse> IsAircraftExistAsync(IHttpClientFactory httpClient, long id, string tailNo)
        {
            string url = $"aircraft/isaircraftexist?id={id}&tailNo={tailNo}";

            CurrentResponse response = await _httpCaller.GetAsync(httpClient, url);

            return response;
        }

        public async Task<CurrentResponse> UploadAircraftImageAsync(IHttpClientFactory httpClient, MultipartFormDataContent fileContent)
        {
            string url = $"aircraft/uploadfile";

            CurrentResponse response = await _httpCaller.PostFileAsync(httpClient, url, fileContent);

            return response;
        }
    }
}
