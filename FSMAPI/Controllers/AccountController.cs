using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Account;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using DataModels.Entities;
using Utilities;
using DataModels.Constants;
using Configuration;
using DataModels.VM.User;
using DataModels.Models;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseAPIController
    {

        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly ISendMailService _sendMailService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly RandomTextGenerator _randomTextGenerator;
        private readonly IUserRolePermissionService _userRolePermissionService;
        private readonly IEmailTokenService _emailTokenService;
        private readonly ConfigurationSettings _configurationSettings;
        private readonly IMyAccountService _myAccountService;

        public AccountController(IAccountService accountService, IUserRolePermissionService userRolePermissionService,
            IUserService userService, ISendMailService sendMailService, IMyAccountService myAccountService,
            IHttpContextAccessor httpContextAccessor, IEmailTokenService emailTokenService)
        {
            _accountService = accountService;
            _userService = userService;
            _sendMailService = sendMailService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _randomTextGenerator = new RandomTextGenerator();
            _userRolePermissionService = userRolePermissionService;
            _emailTokenService = emailTokenService;
            _configurationSettings = ConfigurationSettings.Instance;
            _myAccountService = myAccountService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginVM loginVM)
        {
            loginVM.Password = loginVM.Password.Encrypt();
            CurrentResponse response = _accountService.GetValidUser(loginVM);

            User user = (User)(response.Data);

            if (user != null)
            {
                response = GetDetails(response, user, loginVM.TimeZone);

                return APIResponse(response);
            }

            return APIResponse(response);
        }

        [HttpGet]
        [Route("changecompany")]
        
        public IActionResult ChangeCompany(long userId, int companyId, string timezone)
        {
            CurrentResponse response = _userService.GetById(userId, companyId);
            User user = (User)(response.Data);
            response = GetDetails(response, user, timezone);

            return APIResponse(response);
        }

        private CurrentResponse GetDetails(CurrentResponse response, User user, string userTimeZone)
        {
            response = _userRolePermissionService.GetByRoleId(user.RoleId, user.CompanyId);
            List<UserRolePermissionDataVM> userRolePermissionsList = (List<UserRolePermissionDataVM>)(response.Data);

            string accessToken = _jWTTokenGenerator.Generate(user, userTimeZone == null ? "": userTimeZone);
            string refreshToken = _jWTTokenGenerator.RefreshTokenGenerate();

            SaveRefreshToken(refreshToken, user.Id);

            string timeZone = "";

            if (!string.IsNullOrWhiteSpace(userTimeZone))
            {
                //timeZone =  loginVM.TimeZone.Substring(1, loginVM.TimeZone.Length - 2);
                timeZone = userTimeZone;
            }

            response.Data = new LoginResponseVM
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                CompanyName = user.CompanyName,
                DateofBirth = user.DateofBirth,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                RoleId = user.RoleId,
                Id = user.Id,
                UserPermissionList = userRolePermissionsList,
                ImageURL = user.ImageName,
                LocalTimeZone = timeZone,
                CompanyId = user.CompanyId.GetValueOrDefault()
            };

            return response;
        }

        private void SaveRefreshToken(string refreshToken, long userId)
        {
            EmailToken emailToken = new EmailToken();

            emailToken.EmailType = EmailTypes.RefreshToken;
            emailToken.UserId = userId;
            emailToken.CreatedOn = DateTime.UtcNow;
            emailToken.ExpireOn = DateTime.UtcNow.AddMinutes(_configurationSettings.JWTRefreshTokenExpireDays);
            emailToken.Token = refreshToken;

            _emailTokenService.Create(emailToken);
        }

        [HttpPost]
        [Route("forgotpassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            CurrentResponse response = new CurrentResponse();

            if (ModelState.IsValid)
            {
                string tokenString = _randomTextGenerator.Generate();
                string token = tokenString.Encrypt().Replace("=", "-");

                if (!string.IsNullOrEmpty(token))
                {
                    var url = forgotPasswordVM.ResetURL + token;
                    response = _sendMailService.PasswordReset(forgotPasswordVM.Email, url, token);
                    return APIResponse(response);
                }
            }

            return APIResponse(response);
        }

        [HttpPost]
        [Route("resetpassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordVM model)
        {
            CurrentResponse response = new CurrentResponse();

            if (ModelState.IsValid)
            {
                model.Token = model.Token;
                model.Password = model.Password.Encrypt();
                if (!string.IsNullOrEmpty(model.Token))
                {
                    response = _userService.ResetPassword(model);//try now 
                    return APIResponse(response);
                }
            }

            return APIResponse(response);
        }

        [HttpGet]
        [Route("validatetoken")]
        [AllowAnonymous]
        public IActionResult ValidateToken(string token)
        {
            CurrentResponse response = new CurrentResponse();

            response = _emailTokenService.IsValidToken(token);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("activateaccount")]
        [AllowAnonymous]
        public IActionResult ActivateAccount(string token)
        {
            CurrentResponse response = _accountService.ActivateAccount(token);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("refreshtoken")]
        [AllowAnonymous]
        public IActionResult RefreshToken(string refreshToken, long userId)
        {
            CurrentResponse response = _emailTokenService.ValidateToken(refreshToken, userId);

            if (Convert.ToBoolean(response.Data) == false)
            {
                return APIResponse(response);
            }

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int? companyIdValue = companyId == "" ? null : Convert.ToInt32(companyId);

            response = _userService.FindById(userId,false, companyIdValue);

            UserVM userVM = (UserVM)(response.Data);

            User user = new User();
            user.Id = userVM.Id;
            user.CompanyId = userVM.CompanyId;
            user.RoleId = userVM.RoleId;
            user.RoleName = userVM.Role;

            string accessToken = _jWTTokenGenerator.Generate(user, "");
            refreshToken = _jWTTokenGenerator.RefreshTokenGenerate();

            SaveRefreshToken(refreshToken, user.Id);

            RefreshTokenModel refreshTokenModel = new RefreshTokenModel();

            refreshTokenModel.AccessToken = accessToken;
            refreshTokenModel.RefreshToken = refreshToken;

            response.Data = refreshTokenModel;
            response.Message = "New token granted";
            response.Status = System.Net.HttpStatusCode.OK;

            return APIResponse(response);
        }
    }
}
