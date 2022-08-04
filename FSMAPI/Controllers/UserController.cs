using Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using DataModels.VM.User;
using DataModels.Constants;
using FSMAPI.Utilities;
using System.Security.Claims;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISendMailService _sendMailService;
        private readonly RandomPasswordGenerator _randomPasswordGenerator;
        private readonly RandomTextGenerator _randomTextGenerator;
        private readonly JWTTokenGenerator _jWTTokenGenerator;
        private readonly FileUploader _fileUploader;

        public UserController(IUserService userService, ISendMailService sendMailService, 
            IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _sendMailService = sendMailService;
            _randomPasswordGenerator = new RandomPasswordGenerator();
            _randomTextGenerator = new RandomTextGenerator();
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
            _fileUploader = new FileUploader(webHostEnvironment);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(long id)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            string roleId = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleIdValue = roleId == "" ? 0 : Convert.ToInt32(roleId);

            CurrentResponse response = _userService.GetDetails(id, companyIdValue, roleIdValue);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getmasterdetails")]
        public IActionResult GetMasterDetails(bool isInvited, string token)
        {
            int roleIdValue = (int)DataModels.Enums.UserRole.Admin;
            CurrentResponse response = _userService.GetMasterDetails(roleIdValue, isInvited, token);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public IActionResult Create(UserVM userVM)
        {
            string password = _randomPasswordGenerator.Generate();

            string loggedInUser = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId);

            if (!string.IsNullOrEmpty(loggedInUser))
            {
                userVM.CreatedBy = Convert.ToInt64(loggedInUser);
                userVM.UpdatedBy = Convert.ToInt64(loggedInUser);
            }

            userVM.Password = password.Encrypt();
            CurrentResponse response = _userService.Create(userVM);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                string tokenString = _randomTextGenerator.Generate();
                string token = tokenString.Encrypt().Replace("=", "-");

                userVM.Password = password;

                _sendMailService.NewUserAccountActivation(userVM, token);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(UserVM userVM)
        {
            userVM.UpdatedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _userService.Edit(userVM);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("isemailexist")]
        public IActionResult IsEmailExist(string email)
        {
            CurrentResponse response = _userService.IsEmailExist(email);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            long deletedBy = Convert.ToInt64(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _userService.Delete(id, deletedBy);

            return Ok(response);
        }

        [HttpGet]
        [Route("updatestatus")]
        public IActionResult UpdateStatus(long id, bool isActive)
        {
            CurrentResponse response = _userService.UpdateActiveStatus(id, isActive);

            return Ok(response);
        }

        [HttpPost]
        [Route("list")]
        public IActionResult List(UserDatatableParams datatableParams)
        {
            if(datatableParams.CompanyId == 0)
            {
                string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
                datatableParams.CompanyId = companyId == "" ? 0 : Convert.ToInt32(companyId);
            }

            CurrentResponse response = _userService.List(datatableParams);

            return Ok(response);
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters(int roleId)
        {
            CurrentResponse response = _userService.GetFiltersValue(roleId);

            return Ok(response);
        }

        [HttpGet]
        [Route("findbyid")]
        public IActionResult FindById(long id)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int? companyIdValue = companyId == "" ? null : Convert.ToInt32(companyId);

            string role = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleId = role == "" ? 0 : Convert.ToInt32(role);

            bool isSuperAdmin = roleId == (int)DataModels.Enums.UserRole.SuperAdmin;

            CurrentResponse response = _userService.FindById(id, isSuperAdmin, companyIdValue);

            return Ok(response);
        }

        [HttpGet]
        [Route("findmypreferencesbyid")]
        public IActionResult FindPreferencesById(long id)
        {
            CurrentResponse response = _userService.FindMyPreferencesById(id);

            return Ok(response);
        }

        [HttpGet]
        [Route("listdropdownvaluesbycompanyid")]
        public IActionResult ListDropDownValuesByCompanyId(int companyId)
        {
            CurrentResponse response = _userService.ListDropDownValuesByCompanyId(companyId);

            return Ok(response);
        }

        [HttpPost]
        [Route("uploadprofileimage")]
        public async Task<IActionResult> UploadFileAsync()
        {
            if (!Request.HasFormContentType)
            {
                return Ok(false);
            }

            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            IFormCollection form = Request.Form;

            string fileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHMMss")}_{form["UserId"]}.jpeg";

            if (string.IsNullOrWhiteSpace(companyId))
            {
                companyId = form["CompanyId"];
            }

            bool isFileUploaded = await _fileUploader.UploadAsync(UploadDirectories.UserProfileImage + "\\" + companyId , form, fileName);

            CurrentResponse response = new CurrentResponse();
            response.Data = "";

            if (isFileUploaded)
            {
                response = _userService.UpdateImageName(Convert.ToInt64(form["UserId"]), fileName);
            }

            return Ok(response);
        }
    }
}
