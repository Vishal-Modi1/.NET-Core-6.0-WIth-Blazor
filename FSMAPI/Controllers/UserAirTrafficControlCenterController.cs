using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.Entities;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAirTrafficControlCenterController : BaseAPIController
    {
        private readonly IUserAirTrafficControlCenterService _userAirTrafficControlCenterService;

        public UserAirTrafficControlCenterController(IUserAirTrafficControlCenterService 
            userAirTrafficControlCenterService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _userAirTrafficControlCenterService = userAirTrafficControlCenterService;
        }

        [HttpGet]
        [Route("setDefault")]
        public IActionResult SetDefault(int userAirTrafficControlCenterId)
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);

            UserAirTrafficControlCenter userAirTrafficControl = new UserAirTrafficControlCenter();
            userAirTrafficControl.UserId = Convert.ToInt64(loggedInUser);
            userAirTrafficControl.AirTrafficControlCenterId = userAirTrafficControlCenterId;

            CurrentResponse response = _userAirTrafficControlCenterService.SetDefault(userAirTrafficControl);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _userAirTrafficControlCenterService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
