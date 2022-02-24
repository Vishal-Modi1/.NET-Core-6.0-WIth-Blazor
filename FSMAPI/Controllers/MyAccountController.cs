using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.MyAccount;
using DataModels.VM.Common;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyAccountController : ControllerBase
    {
        private readonly IMyAccountService _myAccountService;

        public MyAccountController(IMyAccountService myAccountService)
        {
            _myAccountService = myAccountService;
        }

        [HttpPost]
        [Route("changepassword")]
        [AllowAnonymous]
        public IActionResult ChangePassword(ChangePasswordVM changePasswordVM)
        {
            changePasswordVM.OldPassword = changePasswordVM.OldPassword.Encrypt();
            changePasswordVM.NewPassword = changePasswordVM.NewPassword.Encrypt();

            CurrentResponse response = _myAccountService.ChangePassword(changePasswordVM);

            return Ok(response);
        }
    }
}
