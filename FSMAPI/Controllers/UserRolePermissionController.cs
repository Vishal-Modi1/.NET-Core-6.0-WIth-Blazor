using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;
using System;
using DataModels.Constants;
using FSMAPI.Utilities;
using System.Security.Claims;

namespace FSMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRolePermissionController : BaseAPIController
    {
        private readonly IUserRolePermissionService _userRolePermissionService;
        private readonly JWTTokenGenerator _jWTTokenGenerator;

        public UserRolePermissionController(IUserRolePermissionService userRolePermissionService, IHttpContextAccessor httpContextAccessor)
        {
            _userRolePermissionService = userRolePermissionService;
            _jWTTokenGenerator = new JWTTokenGenerator(httpContextAccessor.HttpContext);
        }


        [HttpPost]
        [Route("list")]
        public IActionResult List(UserRolePermissionDatatableParams datatableParams)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                int companyId = _jWTTokenGenerator.GetCompanyId();
                if (companyId != datatableParams.CompanyId && datatableParams.CompanyId != 0)
                {
                    return APIResponse(UnAuthorizedResponse.Response());
                }

                datatableParams.CompanyId = companyId;
            }

            CurrentResponse response = _userRolePermissionService.List(datatableParams);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("listbyroleid")]
        public IActionResult ListByRoleId()
        {
            string roleIdClaim = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);

            int companyId = _jWTTokenGenerator.GetCompanyId();
            int roleId = roleIdClaim == "" ? 0 : Convert.ToInt32(roleIdClaim);

            CurrentResponse response = _userRolePermissionService.GetByRoleId(roleId, companyId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("getfilters")]
        public IActionResult GetFilters()
        {
            string roleIdClaim = _jWTTokenGenerator.GetClaimValue(ClaimTypes.Role);
            int roleId = roleIdClaim == "" ? 0 : Convert.ToInt32(roleIdClaim);

            CurrentResponse response = _userRolePermissionService.GetFiltersValue(roleId);

            return APIResponse(response);
        }

        [HttpGet]
        [Route("updatepermission")]
        public IActionResult UpdatePermission(int id, bool isAllow)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                CurrentResponse response = _userRolePermissionService.UpdatePermission(id, isAllow);
                return APIResponse(response);
            }
            else
            {
                return APIResponse(UnAuthorizedResponse.Response());
            }
        }


        [HttpPost]
        [Route("updatepermissions")]
        public IActionResult UpdatePermissions(UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
                {
                    int companyId = _jWTTokenGenerator.GetCompanyId();
                    if (companyId != userRolePermissionFilterVM.CompanyId && userRolePermissionFilterVM.CompanyId != 0)
                    {
                        return APIResponse(UnAuthorizedResponse.Response());
                    }

                    userRolePermissionFilterVM.CompanyId = companyId;
                }

                CurrentResponse response = _userRolePermissionService.UpdatePermissions(userRolePermissionFilterVM);

                return APIResponse(response);

            }
            else
            {
                return APIResponse(UnAuthorizedResponse.Response());
            }
        }

        [HttpGet]
        [Route("updatemobileapppermission")]
        public IActionResult UpdateMobileAppPermission(int id, bool isAllow)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString())
            {
                CurrentResponse response = _userRolePermissionService.UpdateMobileAppPermission(id, isAllow);
                return APIResponse(response);
            }
            else
            {
                return APIResponse(UnAuthorizedResponse.Response());
            }
        }


        [HttpPost]
        [Route("updatemobileapppermissions")]
        public IActionResult UpdateMobileAppPermissions(UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            string role = _jWTTokenGenerator.GetClaimValue(CustomClaimTypes.RoleName);

            if (role.Replace(" ", "") == DataModels.Enums.UserRole.SuperAdmin.ToString() )
            {
                if (role.Replace(" ", "") != DataModels.Enums.UserRole.SuperAdmin.ToString())
                {
                    int companyId = _jWTTokenGenerator.GetCompanyId();
                    if (companyId != userRolePermissionFilterVM.CompanyId && userRolePermissionFilterVM.CompanyId != 0)
                    {
                        return APIResponse(UnAuthorizedResponse.Response());
                    }

                    userRolePermissionFilterVM.CompanyId = companyId;
                }

                CurrentResponse response = _userRolePermissionService.UpdateMobileAppPermissions(userRolePermissionFilterVM);

                return APIResponse(response);
            }
            else
            {
                return APIResponse(UnAuthorizedResponse.Response());
            }
        }
    }
}
