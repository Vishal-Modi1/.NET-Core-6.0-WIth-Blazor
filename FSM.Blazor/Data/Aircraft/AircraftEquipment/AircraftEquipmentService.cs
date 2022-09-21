using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Microsoft.JSInterop;
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

        public async Task<List<AircraftEquipmentDataVM>> ListAsync(DependecyParams dependecyParams, AircraftEquipmentDatatableParams datatableParams)
        {
            dependecyParams.URL = "airCraftequipment/list";

            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);
            List<AircraftEquipmentDataVM> aircraftEquipmentsList = JsonConvert.DeserializeObject<List<AircraftEquipmentDataVM>>(response.Data.ToString());

            return aircraftEquipmentsList;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, AircraftEquipmentsVM airCraftEquipmentsVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(airCraftEquipmentsVM);

            dependecyParams.URL = "airCraftequipment/create";

            if (airCraftEquipmentsVM.Id > 0)
            {
                dependecyParams.URL = "airCraftequipment/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"airCraftequipment/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

       
        private async Task<AircraftEquipmentsVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"airCraftequipment/fetchbyid?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            AircraftEquipmentsVM airCraftEquipmentsVM = new AircraftEquipmentsVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftEquipmentsVM = JsonConvert.DeserializeObject<AircraftEquipmentsVM>(response.Data.ToString());
            }

            return airCraftEquipmentsVM;
        }

        public async Task<AircraftEquipmentsVM> GetEquipmentDetailsAsync(DependecyParams dependecyParams, long id)
        {
            AircraftEquipmentsVM airCraftEquipmentsVM = new AircraftEquipmentsVM();
            airCraftEquipmentsVM.Id = id;
            airCraftEquipmentsVM.StatusList = new List<StatusVM>();
            airCraftEquipmentsVM.ClassificationList = new List<EquipmentClassificationVM>();

            if (id > 0)
            {
                airCraftEquipmentsVM = await GetDetailsAsync(dependecyParams, id);
            }

            airCraftEquipmentsVM.StatusList = await GetStatusListAsync(dependecyParams);
            airCraftEquipmentsVM.ClassificationList = await GetClassificationListAsync(dependecyParams);

            return airCraftEquipmentsVM;
        }

        private async Task<List<StatusVM>> GetStatusListAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "equipmentstatus/list";

            var response = await _httpCaller.GetAsync(dependecyParams);
            List<StatusVM> statusVMList = new List<StatusVM>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                statusVMList = JsonConvert.DeserializeObject<List<StatusVM>>(response.Data.ToString());
            }

            return statusVMList;
        }

        private async Task<List<EquipmentClassificationVM>> GetClassificationListAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "equipmentclassification/list";

            var response = await _httpCaller.GetAsync(dependecyParams);
            List<EquipmentClassificationVM> ClassificationVMList = new List<EquipmentClassificationVM>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                ClassificationVMList = JsonConvert.DeserializeObject<List<EquipmentClassificationVM>>(response.Data.ToString());
            }

            return ClassificationVMList;
        }
    }
}
