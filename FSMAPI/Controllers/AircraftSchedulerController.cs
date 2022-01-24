using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Http;
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
        public IActionResult GetDetails(int id)
        {
            string roleId = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            CurrentResponse response = _aircraftScheduleService.GetDetails(roleIdValue, companyIdValue);
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
    }
}
