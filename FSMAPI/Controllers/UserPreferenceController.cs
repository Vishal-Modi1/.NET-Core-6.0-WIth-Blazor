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
    public class UserPreferenceController : ControllerBase
    {
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceController( IHttpContextAccessor httpContextAccessor, IUserPreferenceService userPreferenceService)
        {
            _userPreferenceService = userPreferenceService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(UserPreferenceVM userPreferenceVM)
        {
            CurrentResponse response = _userPreferenceService.Create(userPreferenceVM);

            return Ok(response);
        }
    }
}
