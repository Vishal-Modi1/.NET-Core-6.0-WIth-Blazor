using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using DataModels.VM.Common;
using DataModels.VM.User;
using DataModels.Constants;

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

        public UserController(IUserService userService, ISendMailService sendMailService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _sendMailService = sendMailService;
            _randomPasswordGenerator = new RandomPasswordGenerator();
            _randomTextGenerator = new RandomTextGenerator();
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("getDetails")]
        public IActionResult GetDetails(int id)
        {
            string companyId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int companyIdValue = companyId == "" ? 0 : Convert.ToInt32(companyId);

            string roleId = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.CompanyId);
            int roleIdValue = companyId == "" ? 0 : Convert.ToInt32(roleId);

            CurrentResponse response = _userService.GetDetails(id, companyIdValue, roleIdValue);
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(UserVM userVM)
        {
            string password = _randomPasswordGenerator.Generate();

            userVM.CreatedBy = userVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

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
            userVM.UpdatedBy = Convert.ToInt32(_jWTTokenGenerator.GetClaimValue(CustomClaimTypes.UserId));

            CurrentResponse response = _userService.Edit(userVM);
            return Ok(response);
        }

        [HttpGet]
        [Route("isemailexist")]
        public IActionResult IsEmailExist(string email)
        {
            CurrentResponse response = _userService.IsEmailExist(email);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(int id)
        {
            CurrentResponse response = _userService.Delete(id);

            return Ok(response);
        }

        [HttpGet]
        [Route("updatestatus")]
        public IActionResult UpdateStatus(int id, bool isActive)
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
    }
}
