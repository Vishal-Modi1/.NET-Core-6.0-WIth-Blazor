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
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public RadarMapConfigurationController(IRadarMapConfigurationService RadarMapConfigurationService, IHttpContextAccessor httpContextAccessor)
        {
            _radarMapConfigurationService = RadarMapConfigurationService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(RadarMapConfigurationVM radarMapConfigurationVM)
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            radarMapConfigurationVM.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _radarMapConfigurationService.SetDefault(radarMapConfigurationVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _radarMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
