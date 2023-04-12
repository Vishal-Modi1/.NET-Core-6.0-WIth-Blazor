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
    public class WindyMapConfigurationController : BaseAPIController
    {
        private readonly IWindyMapConfigurationService _windyMapConfigurationService;

        public WindyMapConfigurationController(IWindyMapConfigurationService WindyMapConfigurationService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _windyMapConfigurationService = WindyMapConfigurationService;
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(WindyMapConfigurationVM windyMapConfigurationVM)
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            windyMapConfigurationVM.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _windyMapConfigurationService.SetDefault(windyMapConfigurationVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _windyMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
