using DataModels.Constants;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using GlobalUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : BaseAPIController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// API for mobile users only
        /// </summary>
        /// 
        [HttpGet]
        [Route("PilotDashboardDetails")]
        public IActionResult PilotDashboardDetails()
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            //if (role.Replace(" ", "") != DataModels.Enums.UserRole.PilotRenter.ToString())
            //{
            //    return Unauthorized();
            //}

            int companyId = _jWTTokenManager.GetCompanyId();
            long userId = _jWTTokenManager.GetUserId();
            string timezone = _jWTTokenManager.GetClaimValue(CustomClaimTypes.TimeZone);
            DateTime userTime = DateConverter.ToLocal(DateTime.UtcNow, timezone);
            
            CurrentResponse response = _dashboardService.PilotDashboardDetails(userId, companyId, userTime);

            return APIResponse(response);
        }
    }
}
