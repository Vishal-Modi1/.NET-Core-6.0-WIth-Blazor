using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace Web.UI.Data.AircraftSchedule
{
    public class AircraftSchedulerService
    {
        private readonly HttpCaller _httpCaller;

        public AircraftSchedulerService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<SchedulerVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"aircraftscheduler/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            SchedulerVM schedulerVM = new SchedulerVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM = JsonConvert.DeserializeObject<SchedulerVM>(response.Data.ToString());
            }

            return schedulerVM;
        }

        public async Task<SchedulerVM> GetDetailsByCompanyIdAsync(DependecyParams dependecyParams, long id, int companyId)
        {
            dependecyParams.URL = $"aircraftscheduler/getDetailsByCompanyId?id={id}&companyId={companyId}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            SchedulerVM schedulerVM = new SchedulerVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM = JsonConvert.DeserializeObject<SchedulerVM>(response.Data.ToString());
            }

            return schedulerVM;
        }

        public async Task<SchedulerVM> GetDropdownValuesByCompanyId(DependecyParams dependecyParams, long id, int companyId)
        {
            dependecyParams.URL = $"aircraftscheduler/getDropdownValuesByCompanyId?companyId={companyId}";

            var response = await _httpCaller.GetAsync(dependecyParams);

            SchedulerVM schedulerVM = new SchedulerVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                schedulerVM = JsonConvert.DeserializeObject<SchedulerVM>(response.Data.ToString());
            }

            return schedulerVM;
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, SchedulerVM schedulerVM, DateTime startTime, DateTime endTime)
        {
            DateTime localStartTime = schedulerVM.StartTime;
            DateTime localEndTime = schedulerVM.EndTime;

            schedulerVM.StartTime = startTime;
            schedulerVM.EndTime = endTime;

            dependecyParams.JsonData = JsonConvert.SerializeObject(schedulerVM);

            schedulerVM.StartTime = localStartTime;
            schedulerVM.EndTime = localEndTime;

            dependecyParams.URL = "aircraftscheduler/create";

            if (schedulerVM.Id > 0)
            {
                dependecyParams.URL = "aircraftscheduler/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<List<SchedulerVM>> ListAsync(DependecyParams dependecyParams, SchedulerFilter schedulerFilter)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(schedulerFilter);
            dependecyParams.URL = "aircraftscheduler/list";

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            if (response == null || response.Status != System.Net.HttpStatusCode.OK)
            {
                return new List<SchedulerVM>();
            }

            List<SchedulerVM> appointmentsList = JsonConvert.DeserializeObject<List<SchedulerVM>>(response.Data.ToString());

            return appointmentsList;
        }

        public async Task<CurrentResponse> DeleteAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"aircraftscheduler/delete?id={id}";
            CurrentResponse response = await _httpCaller.DeleteAsync(dependecyParams);

            return response;
        }

        public async Task<CurrentResponse> UpdateScheduleEndTime(DependecyParams dependecyParams, SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            dependecyParams.URL = $"aircraftscheduler/editendtime";
            dependecyParams.JsonData = JsonConvert.SerializeObject(schedulerEndTimeDetailsVM);

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<List<DropDownLargeValues>> ListActivityTypeDropDownValues(DependecyParams dependecyParams)
        {
            dependecyParams.URL = $"aircraftscheduler/listactivitytypedropdownvalues";
            var response = await _httpCaller.GetAsync(dependecyParams);

            List<DropDownLargeValues> activityList = new List<DropDownLargeValues>();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                activityList = JsonConvert.DeserializeObject<List<DropDownLargeValues>>(response.Data.ToString());
            }

            return activityList;
        }
    }
}
