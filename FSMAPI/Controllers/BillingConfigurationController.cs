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
    public class BillingConfigurationController : BaseAPIController
    {
        private readonly IBillingConfigurationService _billingConfigurationService;
        private readonly JWTTokenManager _jWTTokenManager;

        public BillingConfigurationController(IBillingConfigurationService BillingConfigurationService, IHttpContextAccessor httpContextAccessor)
        {
            _billingConfigurationService = BillingConfigurationService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(BillingConfiguration billingConfiguration)
        {
            CurrentResponse response = _billingConfigurationService.SetDefault(billingConfiguration);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault(int companyId = 0)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") ==  DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                CurrentResponse response = _billingConfigurationService.FindByUserId(companyId);

                return APIResponse(response);
            }
            else
            {
                string companyIdValue = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);
                CurrentResponse response = _billingConfigurationService.FindByUserId(Convert.ToInt32(companyIdValue));

                return APIResponse(response);
            }
        }
    }
}
