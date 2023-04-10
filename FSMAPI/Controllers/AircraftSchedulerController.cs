using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Security.Claims;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftSchedulerController : BaseAPIController
    {
        private readonly JWTTokenManager _jWTTokenManager;
        private readonly IAircraftScheduleService _aircraftScheduleService;
        private ExternalAPICaller _externalAPICaller { get; set; }

        public AircraftSchedulerController(IAircraftScheduleService aircraftScheduleService, IHttpContextAccessor httpContextAccessor)
        {
            _aircraftScheduleService = aircraftScheduleService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
            _externalAPICaller = new ExternalAPICaller();
        }

        [HttpGet]
        [Route("getDetails")]
        public async Task<IActionResult> GetDetailsAsync(long id)
        {
            string roleId = _jWTTokenManager.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            string userId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            long userIdValue = userId == "" ? 0 : Convert.ToInt64(userId);

            string companyId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _aircraftScheduleService.GetDetails(roleIdValue, companyIdValue, id, userIdValue);

            return APIResponse(response);
        }


        //For Super Admin
        [HttpGet]
        [Route("getDetailsByCompanyId")]
        public IActionResult GetDetailsByCompanyId(long id, int companyId)
        {
            string roleId = _jWTTokenManager.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            string userId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            long userIdValue = userId == "" ? 0 : Convert.ToInt64(userId);

            CurrentResponse response = _aircraftScheduleService.GetDetails(roleIdValue, companyId, id, userIdValue);
            return APIResponse(response);
        }

        [HttpGet]
        [Route("getDropdownValuesByCompanyId")]
        public IActionResult GetDropdownValuesByCompanyId(int companyId)
        {
            string roleId = _jWTTokenManager.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            string userId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            long userIdValue = userId == "" ? 0 : Convert.ToInt64(userId);

            CurrentResponse response = _aircraftScheduleService.GetDropdownValuesByCompanyId(roleIdValue, companyId, userIdValue);
            return APIResponse(response);
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create(SchedulerVM schedulerVM)
        {
            //if (schedulerVM.CompanyId == 0)
            //{
            //    schedulerVM.CompanyId = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId));
            //}

            //if (schedulerVM.UserId == 0)
            //{
            //    schedulerVM.UserId = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            //}

            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                schedulerVM.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            schedulerVM.CreatedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleService.Create(schedulerVM, timezone);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(SchedulerFilter schedulerFilter)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                schedulerFilter.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _aircraftScheduleService.List(schedulerFilter);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(SchedulerVM schedulerVM)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                schedulerVM.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            schedulerVM.UpdatedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);

            CurrentResponse response = _aircraftScheduleService.Edit(schedulerVM, timezone);
            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _aircraftScheduleService.Delete(id, deletedBy);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("editEndTime")]
        public IActionResult EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            schedulerEndTimeDetailsVM.UpdatedBy = Convert.ToInt32(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleService.EditEndTime(schedulerEndTimeDetailsVM);
            return APIResponse(response);
        }

        [HttpGet]
        [Route("listActivitytypeDropdownValues")]
        public IActionResult ListActivityTypeDropDownValues()
        {
            string roleId = _jWTTokenManager.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            CurrentResponse response = _aircraftScheduleService.ListActivityTypeDropDownValues(roleIdValue);
            return APIResponse(response);
        }

        //private async Task SetAirportValues(SchedulerVM schedulerVM)
        //{
        //    if (schedulerVM == null || schedulerVM.DepartureAirportId == null || schedulerVM.ArrivalAirportId == null)
        //    {
        //        return;
        //    }

        //    // Departure Airport
        //    string url = $"{ConfigurationSettings.Instance.AirportAPIURL}&id={schedulerVM.DepartureAirportId}";
        //    HttpResponseMessage responseObject = await _externalAPICaller.Get(url);

        //    if (!responseObject.IsSuccessStatusCode)
        //    {
        //        return;
        //    }

        //    string responseJson = await responseObject.Content.ReadAsStringAsync();
        //    AirportViewModel airportViewModel = JsonConvert.DeserializeObject<AirportViewModel>(responseJson);

        //    if (airportViewModel.Value.Count() > 0)
        //    {
        //        schedulerVM.DepartureAirport = airportViewModel.Value.FirstOrDefault().Name;
        //    }

        //    // Arrival Airport
        //    url = $"{ConfigurationSettings.Instance.AirportAPIURL}&id={schedulerVM.ArrivalAirportId}";
        //    responseObject = await _externalAPICaller.Get(url);

        //    if (!responseObject.IsSuccessStatusCode)
        //    {
        //        return;
        //    }

        //    responseJson = await responseObject.Content.ReadAsStringAsync();
        //    airportViewModel = JsonConvert.DeserializeObject<AirportViewModel>(responseJson);

        //    if (airportViewModel.Value.Count() > 0)
        //    {
        //        schedulerVM.ArrivalAirport = airportViewModel.Value.FirstOrDefault().Name;
        //    }
        //}
    }
}
