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
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly IAircraftScheduleService _aircraftScheduleService;

        public AircraftSchedulerController(IAircraftScheduleService aircraftScheduleService, IHttpContextAccessor httpContextAccessor)
        {
            _aircraftScheduleService = aircraftScheduleService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            string roleId = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            string userId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            long userIdValue = userId == "" ? 0 : Convert.ToInt64(userId);

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _aircraftScheduleService.GetDetails(roleIdValue, companyIdValue, id, userIdValue);

            return APIResponse(response);
        }


        //For Super Admin
        [HttpGet]
        [Route("getDetailsByCompanyId")]
        public IActionResult GetDetailsByCompanyId(long id, int companyId)
        {
            string roleId = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            string userId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            long userIdValue = userId == "" ? 0 : Convert.ToInt64(userId);

            CurrentResponse response = _aircraftScheduleService.GetDetails(roleIdValue, companyId, id, userIdValue);
            return APIResponse(response);
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create(SchedulerVM schedulerVM)
        {
            //if (schedulerVM.CompanyId == 0)
            //{
            //    schedulerVM.CompanyId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId));
            //}

            //if (schedulerVM.UserId == 0)
            //{
            //    schedulerVM.UserId = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            //}

            schedulerVM.CreatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleService.Create(schedulerVM);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(SchedulerFilter schedulerFilter)
        {
            if (schedulerFilter.CompanyId == 0)
            {
                string companyIdValue = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                schedulerFilter.CompanyId = companyIdValue == "" ? 0 : Convert.ToInt32(companyIdValue);
            }

            CurrentResponse response = _aircraftScheduleService.List(schedulerFilter);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(SchedulerVM schedulerVM)
        {
            schedulerVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleService.Edit(schedulerVM);
            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            long deletedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _aircraftScheduleService.Delete(id, deletedBy);

            return APIResponse(response);
        }

        [HttpPost]
        [Route("editEndTime")]
        public IActionResult EditEndTime(SchedulerEndTimeDetailsVM schedulerEndTimeDetailsVM)
        {
            schedulerEndTimeDetailsVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleService.EditEndTime(schedulerEndTimeDetailsVM);
            return APIResponse(response);
        }

        [HttpGet]
        [Route("listActivitytypeDropdownValues")]
        public IActionResult ListActivityTypeDropDownValues()
        {
            string roleId = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            CurrentResponse response = _aircraftScheduleService.ListActivityTypeDropDownValues(roleIdValue);
            return APIResponse(response);
        }

    }
}
