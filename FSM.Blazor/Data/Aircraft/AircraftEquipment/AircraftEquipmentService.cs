using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using DataModels.VM.AircraftEquipment;

namespace FSM.Blazor.Data.Aircraft.AircraftEquipment
{
    public class AircraftEquipmentService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftEquipmentService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<AircraftEquipmentDataVM>> ListAsync(IHttpClientFactory httpClient, AircraftEquipmentDatatableParams datatableParams)
        {
            string url = "airCraftequipment/list";

            string jsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);
            List<AircraftEquipmentDataVM> aircraftEquipmentsList = JsonConvert.DeserializeObject<List<AircraftEquipmentDataVM>>(response.Data);

            return aircraftEquipmentsList;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, AirCraftEquipmentsVM airCraftEquipmentsVM)
        {
            string jsonData = JsonConvert.SerializeObject(airCraftEquipmentsVM);

            string url = "airCraftequipment/create";

            if (airCraftEquipmentsVM.Id > 0)
            {
                url = "airCraftequipment/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(httpClient, url, jsonData);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, int id)
        {
            string url = $"airCraftequipment/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

       
        public async Task<AirCraftEquipmentsVM> GetDetailsAsync(IHttpClientFactory httpClient, int id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"airCraftequipment/fetchbyid?id={id}");

            AirCraftEquipmentsVM airCraftEquipmentsVM = new AirCraftEquipmentsVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftEquipmentsVM = JsonConvert.DeserializeObject<AirCraftEquipmentsVM>(response.Data);
            }

            return airCraftEquipmentsVM;
        }
    }
}
