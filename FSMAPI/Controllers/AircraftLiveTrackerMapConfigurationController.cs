using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;
using DataModels.VM.Weather;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AircraftLiveTrackerMapConfigurationController : BaseAPIController
    {
        private readonly IAircraftLiveTrackerMapConfigurationService _aircraftLiveTrackerMapConfigurationService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public AircraftLiveTrackerMapConfigurationController(IAircraftLiveTrackerMapConfigurationService AircraftLiveTrackerMapConfigurationService, IHttpContextAccessor httpContextAccessor)
        {
            _aircraftLiveTrackerMapConfigurationService = AircraftLiveTrackerMapConfigurationService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(AircraftLiveTrackerMapConfigurationVM aircraftLiveTrackerMapConfigurationVM)
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            aircraftLiveTrackerMapConfigurationVM.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _aircraftLiveTrackerMapConfigurationService.SetDefault(aircraftLiveTrackerMapConfigurationVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _aircraftLiveTrackerMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
