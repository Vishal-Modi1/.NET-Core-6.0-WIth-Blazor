using DataModels.VM.InstructorType;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using DataModels.VM.Common;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FSM.Blazor.Data.InstructorType
{
    public class InstructorTypeService
    {
        private readonly HttpCaller _httpCaller;

        public InstructorTypeService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<InstructorTypeVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
            dependecyParams.URL = "instructorType/List";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<InstructorTypeVM>();
            }

            List<InstructorTypeVM> instructorTypes = JsonConvert.DeserializeObject<List<InstructorTypeVM>>(response.Data.ToString());

            return instructorTypes; 
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, InstructorTypeVM instructorTypeVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(instructorTypeVM);

            dependecyParams.URL = "instructorType/create";

            if (instructorTypeVM.Id > 0)
            {
                dependecyParams.URL = "instructorType/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, int id)
        {
            dependecyParams.URL = $"instructorType/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }
    }
}
