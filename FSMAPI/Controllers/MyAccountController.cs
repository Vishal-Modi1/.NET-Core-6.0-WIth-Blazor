using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.MyAccount;
using DataModels.VM.Common;
using DataModels.Constants;
using System.Security.Claims;
using FSMAPI.Utilities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyAccountController : BaseAPIController
    {
        private readonly IMyAccountService _myAccountService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public MyAccountController(IMyAccountService myAccountService, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _myAccountService = myAccountService;
        }

        [HttpPost]
        [Route("changepassword")]
    
        public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            changePasswordVM.OldPassword = changePasswordVM.OldPassword.Encrypt();
            changePasswordVM.NewPassword = changePasswordVM.NewPassword.Encrypt();

            CurrentResponse response = _myAccountService.ChangePassword(changePasswordVM);

            return APIResponse(response);
        }

        /// <summary>
        /// API for mobile users only
        /// </summary>

        [HttpGet]
        [Route("myprofiledetails")]
        
        public IActionResult MyProfileDetilas()
        {
            string companyIdValue = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyId = companyIdValue == "" ? 0 : Convert.ToInt32(companyIdValue);
            long id = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            int roleId = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(ClaimTypes.Role));

            CurrentResponse response = _myAccountService.GetMyProfileDetails(companyId, roleId, id);

            return APIResponse(response);
        }
    }
}
