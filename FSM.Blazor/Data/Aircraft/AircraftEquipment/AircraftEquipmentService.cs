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
            List<AircraftEquipmentDataVM> aircraftEquipmentsList = JsonConvert.DeserializeObject<List<AircraftEquipmentDataVM>>(response.Data.ToString());

            return aircraftEquipmentsList;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(IHttpClientFactory httpClient, AircraftEquipmentsVM airCraftEquipmentsVM)
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

        public async Task<CurrentResponse> DeleteAsync(IHttpClientFactory httpClient, long id)
        {
            string url = $"airCraftequipment/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(httpClient, url);

            return response;
        }

       
        private async Task<AircraftEquipmentsVM> GetDetailsAsync(IHttpClientFactory httpClient, long id)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"airCraftequipment/fetchbyid?id={id}");

            AircraftEquipmentsVM airCraftEquipmentsVM = new AircraftEquipmentsVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftEquipmentsVM = JsonConvert.DeserializeObject<AircraftEquipmentsVM>(response.Data.ToString());
            }

            return airCraftEquipmentsVM;
        }

        public async Task<AircraftEquipmentsVM> GetEquipmentDetailsAsync(IHttpClientFactory httpClient, long id)
        {
            AircraftEquipmentsVM airCraftEquipmentsVM = new AircraftEquipmentsVM();
            airCraftEquipmentsVM.Id = id;
            airCraftEquipmentsVM.statusList = new List<StatusVM>();
            airCraftEquipmentsVM.classificationList = new List<EquipmentClassificationVM>();

            if (id > 0)
            {
                airCraftEquipmentsVM = await GetDetailsAsync(httpClient, id);
            }

            airCraftEquipmentsVM.statusList = await GetStatusListAsync(httpClient);
            airCraftEquipmentsVM.classificationList = await GetClassificationListAsync(httpClient);

            return airCraftEquipmentsVM;
        }

        private async Task<List<StatusVM>> GetStatusListAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient,$"equipmentstatus/list");
            List<StatusVM> statusVMList = new List<StatusVM>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                statusVMList = JsonConvert.DeserializeObject<List<StatusVM>>(response.Data.ToString());
            }

            return statusVMList;
        }

        private async Task<List<EquipmentClassificationVM>> GetClassificationListAsync(IHttpClientFactory httpClient)
        {
            var response = await _httpCaller.GetAsync(httpClient, $"equipmentclassification/list");
            List<EquipmentClassificationVM> ClassificationVMList = new List<EquipmentClassificationVM>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                ClassificationVMList = JsonConvert.DeserializeObject<List<EquipmentClassificationVM>>(response.Data.ToString());
            }

            return ClassificationVMList;
        }
    }
}
