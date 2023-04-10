using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using FSMAPI.Utilities;
using DataModels.Constants;
using Microsoft.AspNetCore.Authorization;
using DataModels.VM.Company.Settings;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyDateFormatController : BaseAPIController
    {
        private readonly ICompanyDateFormatService _companyDateFormatService;
        private readonly JWTTokenManager _jWTTokenManager;

        public CompanyDateFormatController(ICompanyDateFormatService CompanyDateFormatService, IHttpContextAccessor httpContextAccessor)
        {
            _companyDateFormatService = CompanyDateFormatService;
            _jWTTokenManager = new JWTTokenManager(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("setDefault")]
        public IActionResult SetDefault(CompanyDateFormatVM companyDateFormatVM)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyDateFormatVM.CompanyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _companyDateFormatService.SetDefault(companyDateFormatVM);

            return APIResponse(response);
        }


        [HttpGet]
        [Route("getDefault")]
        public IActionResult GetDefault(int companyId)
        {
            string role = _jWTTokenManager.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                companyId = _jWTTokenManager.GetCompanyId();
            }

            CurrentResponse response = _companyDateFormatService.FindByCompanyId(companyId);
            
            return APIResponse(response);
        }

        [HttpGet]
        [Route("listDropdownValues")]
        public IActionResult ListAircraftDropdownValues()
        {
            CurrentResponse response = _companyDateFormatService.ListDropDownValues();
            return APIResponse(response);
        }
    }
}
