using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Security.Claims;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftSchedulerController : ControllerBase
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

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _aircraftScheduleService.GetDetails(roleIdValue, companyIdValue, id);
            return Ok(response);
        }


        [HttpPost]
        [Route("create")]
        public IActionResult Create(SchedulerVM schedulerVM)
        {
            schedulerVM.CreatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _aircraftScheduleService.Create(schedulerVM);

            return Ok(response);
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

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(SchedulerVM schedulerVM)
        {
            CurrentResponse response = _aircraftScheduleService.Edit(schedulerVM);
            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            CurrentResponse response = _aircraftScheduleService.Delete(id);

            return Ok(response);
        }

        [HttpGet]
        [Route("isaircraftalreadycheckout")]
        public IActionResult IsAircraftAlreadyCheckOut(long aircraftId)
        {
            CurrentResponse response = _aircraftScheduleService.IsAircraftAlreadyCheckOut(aircraftId);

            return Ok(response);
        }
    }
}
