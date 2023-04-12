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
    public class RadarMapConfigurationController : BaseAPIController
    {
        private readonly IRadarMapConfigurationService _radarMapConfigurationService;

        public RadarMapConfigurationController(IRadarMapConfigurationService RadarMapConfigurationService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _radarMapConfigurationService = RadarMapConfigurationService;
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(RadarMapConfigurationVM radarMapConfigurationVM)
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            radarMapConfigurationVM.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _radarMapConfigurationService.SetDefault(radarMapConfigurationVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _radarMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
