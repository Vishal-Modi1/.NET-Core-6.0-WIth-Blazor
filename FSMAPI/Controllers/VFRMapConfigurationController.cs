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
    public class VFRMapConfigurationController : BaseAPIController
    {
        private readonly IVFRMapConfigurationService _vFRMapConfigurationService;

        public VFRMapConfigurationController(IVFRMapConfigurationService vFRMapConfigurationService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _vFRMapConfigurationService = vFRMapConfigurationService;
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(VFRMapConfigurationVM vFRMapConfigurationVM)
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            vFRMapConfigurationVM.UserId = Convert.ToInt64(loggedInUser);
            CurrentResponse response = _vFRMapConfigurationService.SetDefault(vFRMapConfigurationVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault()
        {
            string loggedInUser = _jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId);
            CurrentResponse response = _vFRMapConfigurationService.FindByUserId(Convert.ToInt64(loggedInUser));
            
            return APIResponse(response);
        }
    }
}
