using DataModels.VM.Aircraft;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using Web.UI.Shared;
using DE = DataModels.Entities;

namespace Web.UI.Data.Aircraft
{
    public class AircraftService
    {
        private readonly HttpCaller _httpCaller;
        
        //[CascadingParameter]
        //public Error? Error { get; set; }

        public AircraftService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<AircraftDataVM>> ListAsync(DependecyParams dependecyParams, AircraftDatatableParams datatableParams)
        {
            try
            {
                dependecyParams.URL = "aircraft/list";
                dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
                CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);
                List<AircraftDataVM> aircraftList = JsonConvert.DeserializeObject<List<AircraftDataVM>>(response.Data.ToString());

                return aircraftList;
            }
            catch(Exception exc)
            {
                //Error?.ProcessError(exc);
                return null;
            }
        }

        public async Task<List<DE.Aircraft>> ListAllAsync(DependecyParams dependecyParams, int companyId)
        {
            try
            {
                dependecyParams.URL = $"aircraft/listall?companyId={companyId}";

                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);
                List<DE.Aircraft> aircraftList = JsonConvert.DeserializeObject<List<DE.Aircraft>>(response.Data.ToString());

                return aircraftList;
            }
            catch (Exception exc)
            {
                //Error?.ProcessError(exc);
                return null;
            }
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, AircraftVM aircraftVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(aircraftVM);

            dependecyParams.URL = "aircraft/create";

            if (aircraftVM.Id > 0)
            {
                dependecyParams.URL = "aircraft/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);
           
            return response;
        }

        public async Task<CurrentResponse> SaveandUpdateEquipmentTimeListAsync(DependecyParams dependecyParams, AircraftVM aircraftVM)
        {
            dependecyParams.URL = "aircraft/createaircraftequipment";

            dependecyParams.JsonData = JsonConvert.SerializeObject(aircraftVM.AircraftEquipmentTimesList);
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"aircraft/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<AircraftFilterVM> GetFiltersAsync(DependecyParams dependecyParams)
        {
            dependecyParams.URL = "aircraft/getfilters";
            var response = await _httpCaller.GetAsync(dependecyParams);

            AircraftFilterVM aircraftFilterVM = new AircraftFilterVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                aircraftFilterVM = JsonConvert.DeserializeObject<AircraftFilterVM>(response.Data.ToString());
            }

            return aircraftFilterVM;
        }

        public async Task<AircraftVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"aircraft/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            AircraftVM airCraftVM = new AircraftVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftVM = JsonConvert.DeserializeObject<AircraftVM>(response.Data.ToString());
            }

            return airCraftVM;
        }

        public async Task<List<DropDownLargeValues>> ListDropdownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"aircraft/listdropdownvalues";
            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownLargeValues> airCraftList = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftList = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return airCraftList;
        }

        public async Task<List<DropDownLargeValues>> ListDropdownValuesByCompanyId(DependecyParams dependecyParams, int companyId)
        {
            dependecyParams.URL = $"aircraft/listdropdownvaluesbycompanyid?companyId={companyId}"; 
            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownLargeValues> airCraftList = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                airCraftList = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return airCraftList;
        }

        public async Task<CurrentResponse> IsAircraftExistAsync(DependecyParams dependecyParams, long id, string tailNo)
        {
            dependecyParams.URL = $"aircraft/isaircraftexist?id={id}&tailNo={tailNo}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UploadAircraftImageAsync(DependecyParams dependecyParams, MultipartFormDataContent fileContent)
        {
            dependecyParams.URL = $"aircraft/uploadfile";

            CurrentResponse response = await _httpCaller.PostFileAsync(dependecyParams, fileContent);

            return response;
        }

        public async Task<CurrentResponse> UpdateStatus(DependecyParams dependecyParams, long id, byte statusId)
        {
            dependecyParams.URL = $"aircraft/updatestatus?id={id}&statusId={statusId}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> LockAircraft(DependecyParams dependecyParams, long id, bool isLock)
        {
            dependecyParams.URL = $"aircraft/updatestatus?id={id}&isLock={isLock}";
            CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

            return response;
        }
    }
}
