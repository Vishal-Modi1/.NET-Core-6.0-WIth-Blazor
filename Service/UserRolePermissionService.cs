using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.UserRolePermission;

namespace Service
{
    public class UserRolePermissionService : BaseService, IUserRolePermissionService
    {
        private readonly IUserRolePermissionRepository _userRolePermissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IModuleDetailsRepository _moduleDetailsRepo;
        private readonly ICompanyRepository _companyRepository;

        public UserRolePermissionService(IUserRolePermissionRepository userRolePermissionRepository, 
            IUserRoleRepository userRoleRepository, IModuleDetailsRepository moduleDetailsRepo, ICompanyRepository companyRepository)
        {
            _userRolePermissionRepository = userRolePermissionRepository;
            _userRoleRepository = userRoleRepository;
            _moduleDetailsRepo = moduleDetailsRepo;
            _companyRepository = companyRepository;
        }

        public CurrentResponse GetByRoleId(int roleId, int? companyId)
        {
            try
            {
                List<UserRolePermissionDataVM> userRolePermissionsList = _userRolePermissionRepository.GetByRoleId(roleId, companyId);
                CreateResponse(userRolePermissionsList, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(UserRolePermissionDatatableParams datatableParams)
        {
            try
            {
                List<UserRolePermissionDataVM> userRolePermissionList = _userRolePermissionRepository.List(datatableParams);
                CreateResponse(userRolePermissionList, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdatePermission(int id, bool isAllow)
        {
            try
            {
                _userRolePermissionRepository.UpdatePermission(id, isAllow);

                string message = "Permission granted successfully.";

                if (!isAllow)
                {
                    message = "Permission denied successfully.";
                }

                CreateResponse(true, HttpStatusCode.OK, message);

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetFiltersValue(int roleId)
        {
            try
            {
                UserRolePermissionFilterVM userRolePermissionFilterVM = new UserRolePermissionFilterVM();

                userRolePermissionFilterVM.UserRoles = _userRoleRepository.ListDropDownValues(roleId);
                userRolePermissionFilterVM.ModuleList = _moduleDetailsRepo.ListDropDownValues();
                userRolePermissionFilterVM.Companies = _companyRepository.ListDropDownValues();

                CreateResponse(userRolePermissionFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new UserRolePermissionFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdatePermissions(UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            try
            {
                _userRolePermissionRepository.UpdatePermissions(userRolePermissionFilterVM);

                string message = "Permissions granted successfully.";

                if (!userRolePermissionFilterVM.IsAllow)
                {
                    message = "Permissions denied successfully.";
                }

                CreateResponse(true, HttpStatusCode.OK, message);

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateMobileAppPermission(int id, bool isAllow)
        {
            try
            {
                _userRolePermissionRepository.UpdateMobileAppPermission(id, isAllow);

                string message = "Permission granted successfully.";

                if (!isAllow)
                {
                    message = "Permission denied successfully.";
                }

                CreateResponse(true, HttpStatusCode.OK, message);

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateMobileAppPermissions(UserRolePermissionFilterVM userRolePermissionFilterVM)
        {
            try
            {
                _userRolePermissionRepository.UpdateMobileAppPermissions(userRolePermissionFilterVM);

                string message = "Permissions granted successfully.";

                if (!userRolePermissionFilterVM.IsAllow)
                {
                    message = "Permissions denied successfully.";
                }

                CreateResponse(true, HttpStatusCode.OK, message);

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
