using DataModels.Constants;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.User;
using FSMAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interface;
using System.Security.Claims;
using GlobalUtilities;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InviteUserController : BaseAPIController
    {
        private readonly IInviteUserService _inviteUserService;
        private readonly RandomTextGenerator _randomTextGenerator;
        private readonly ISendMailService _sendMailService;
        private readonly ICompanyService _companyService;

        public InviteUserController(IInviteUserService inviteUser, ISendMailService sendMailService,
            IHttpContextAccessor httpContextAccessor, ICompanyService companyService) : base(httpContextAccessor)
        {
            _inviteUserService = inviteUser;
            _randomTextGenerator = new RandomTextGenerator();
            _sendMailService = sendMailService;
            _companyService = companyService;
        }

        [HttpGet]
        [Route("getdetails")]
        public IActionResult GetDetails(long id)
        {
            string role = _jWTTokenManager.GetClaimValue(ClaimTypes.Role);
            int roleId = Convert.ToInt32(role);

            CurrentResponse response = _inviteUserService.GetDetails(roleId, id);
            return APIResponse(response);
        }

        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(InviteUserVM inviteUserVM)
        {
            CurrentResponse response = _inviteUserService.Edit(inviteUserVM);
            return APIResponse(response);
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(InviteUserVM inviteUserVM)
        {
            if (inviteUserVM.CompanyId == 0)
            {
                string company = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);
                inviteUserVM.CompanyId = Convert.ToInt32(company);
            }

            CurrentResponse response = _inviteUserService.IsValidInvite(inviteUserVM);

            bool isValid = (bool)response.Data;

            if (!isValid)
            {
                return APIResponse(response);
            }

            string loggedInUser = _jWTTokenManager.GetClaimValue(ClaimTypes.Role);
            long userId = Convert.ToInt64(loggedInUser);

            inviteUserVM.InvitedBy = userId;

            response = _inviteUserService.Create(inviteUserVM);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                InviteUser inviteUser = (InviteUser)(response.Data);

                string tokenString = _randomTextGenerator.Generate();
                string token = tokenString.Encrypt().Replace("=", "-");

                UserVM userVM = new UserVM();

                userVM.Email = inviteUserVM.Email;
                userVM.CompanyName = _companyService.FindByCondition(p => p.Id == inviteUserVM.CompanyId).Name;
                userVM.Role = inviteUserVM.ListRoles.Where(p => p.Id == inviteUserVM.RoleId).First().Name;
                userVM.ActivationLink = inviteUserVM.ActivationLink;

                _sendMailService.InviteUser(userVM, token, inviteUser.Id);
            }

            return APIResponse(response);
        }

        [HttpGet]
        [Route("acceptinvitation")]
        [AllowAnonymous]
        public IActionResult AcceptInvitation(string token)
        {
            CurrentResponse response  =  _inviteUserService.AcceptInvitation(token);

            return APIResponse(response);
        }


        [HttpPost]
        [Route("list")]
        public IActionResult List(UserDatatableParams datatableParams)
        {
            string companyId = _jWTTokenManager.GetClaimValue(CustomClaimTypes.CompanyId);

            if (!string.IsNullOrWhiteSpace(companyId))
            {
                datatableParams.CompanyId = Convert.ToInt32(companyId);
            }

            CurrentResponse response = _inviteUserService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(long id)
        {
            long deletedBy = Convert.ToInt64(_jWTTokenManager.GetClaimValue(CustomClaimTypes.UserId));
            CurrentResponse response = _inviteUserService.Delete(id, deletedBy);

            return APIResponse(response);
        }

    }
}
