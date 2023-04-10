using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPreferenceController : BaseAPIController
    {
        private readonly JWTTokenManager _jWTTokenManager;
        private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceController( IHttpContextAccessor httpContextAccessor, IUserPreferenceService userPreferenceService)
        {
            _userPreferenceService = userPreferenceService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(UserPreferenceVM userPreferenceVM)
        {
            userPreferenceVM.UserId = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _userPreferenceService.Create(userPreferenceVM);

            return APIResponse(response);
        }
    }
}
