using GlobalUtilities;
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

        public MyAccountController(IMyAccountService myAccountService,
            IHttpContextAccessor httpContextAccessor, 
            IWebHostEnvironment webHostEnvironment) : base(httpContextAccessor, webHostEnvironment)
        {
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
        
        public IActionResult MyProfileDetails()
        {
            string companyIdValue = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyId = companyIdValue == "" ? 0 : Convert.ToInt32(companyIdValue);
            long id = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            int roleId = Convert.ToInt32(_jWTTokenManager.GetClaimValue(ClaimTypes.Role));

            CurrentResponse response = _myAccountService.GetMyProfileDetails(companyId, roleId, id);

            return APIResponse(response);
        }
    }
}
