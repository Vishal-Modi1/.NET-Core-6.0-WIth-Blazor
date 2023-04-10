using Repository.Interface;
using Service.Interface;
using System.Net;
using DataModels.VM.MyAccount;
using DataModels.VM.Common;
using System;
using DataModels.VM.Document;
using System.Collections.Generic;
using DataModels.Entities;
using DataModels.VM.Reservation;
using DataModels.VM.MobileAppVM;

namespace Service
{
    public class MyAccountService : BaseService, IMyAccountService
    {
        private readonly IMyAccountRepository _myAccountRepository;
        private readonly IUserService _userService;
        private readonly IDocumentService _documentService;
        private readonly IUserRoleRepository _roleRepository;
        private readonly IModuleDetailsRepository _moduleDetailsRepository;
        private readonly IReservationRepository _reservationRepository;

        public MyAccountService(IMyAccountRepository myAccountRepository, IUserService userService, 
            IDocumentService documentService, IUserRoleRepository roleRepository, IModuleDetailsRepository moduleDetailsRepository,
            IReservationRepository reservationRepository)
        {
            _myAccountRepository = myAccountRepository;
            _userService = userService;
            _documentService = documentService;
            _roleRepository = roleRepository;
            _moduleDetailsRepository = moduleDetailsRepository;
            _reservationRepository = reservationRepository;
        }

        public CurrentResponse ChangePassword(ChangePasswordVM changePasswordVM)
        {
            bool isValidOldPassword = _myAccountRepository.IsValidOldPassword(changePasswordVM.OldPassword, changePasswordVM.UserId);

            if(!isValidOldPassword)
            {
                CreateResponse(isValidOldPassword, HttpStatusCode.ExpectationFailed, "Old password is invalid");
                return _currentResponse;
            }

            bool isPasswordChange = _myAccountRepository.ChangePassword(changePasswordVM);

            CreateResponse(isPasswordChange, HttpStatusCode.OK, "Password has been updated successfully");

            return _currentResponse;
        }

        public CurrentResponse FindById(long id)
        {
            throw new System.NotImplementedException();
        }

        public CurrentResponse GetMyProfileDetails(int companyId, int roleId, long userId)
        {
            MyProfileDetailsVM myProfileDetailsVM = new MyProfileDetailsVM();
            myProfileDetailsVM.DocumentDetails = new DocumentDetails();
            myProfileDetailsVM.ReservationDetails = new ReservationDetails();
            try
            {
                UserRole userRole = _roleRepository.FindById(roleId);

                myProfileDetailsVM.UserDetails = _userService.GetUserDetails(userId, companyId, roleId);
                myProfileDetailsVM.DocumentDetails.DocumentDataListVM = GetDocumentDetails(userId, companyId, userRole);
                myProfileDetailsVM.ReservationDetails.ReservationDataListVM = GetReservationDetails(userId, companyId, userRole);

                CreateResponse(myProfileDetailsVM, HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());
            }

            return _currentResponse;
        }

        private List<DocumentDataVM> GetDocumentDetails(long userId, int companyId, UserRole userRole)
        {
            ModuleDetail moduleDetail = _moduleDetailsRepository.FindByName("User");

            DocumentDatatableParams datatableParams = new DocumentDatatableParams();

            datatableParams.Start = 1;
            datatableParams.Length = 10;
            datatableParams.UserId = userId;
            datatableParams.SortOrderColumn = "CreatedOn";
            datatableParams.OrderType = "ASC";
            datatableParams.UserRole = Enum.Parse<DataModels.Enums.UserRole>(userRole.Name.Replace(" ",""));
            datatableParams.ModuleId = moduleDetail.Id;

            if (datatableParams.CompanyId == 0 && datatableParams.UserRole != DataModels.Enums.UserRole.SuperAdmin)
            {
                datatableParams.CompanyId = companyId;
            }

            if (datatableParams.UserId == 0 && datatableParams.UserRole != DataModels.Enums.UserRole.SuperAdmin &&
                datatableParams.UserRole != DataModels.Enums.UserRole.Admin)
            {
                datatableParams.UserId = userId;
            }

            List<DocumentDataVM> documentsList = _documentService.ListDetails(datatableParams);

            return documentsList;
        }

        private List<ReservationDataVM> GetReservationDetails(long userId, int companyId, UserRole userRole)
        {
            ReservationDataTableParams datatableParams = new ReservationDataTableParams();

            datatableParams.Start = 1;
            datatableParams.Length = 10;
            datatableParams.SortOrderColumn = "DisplayName";
            datatableParams.OrderType = "ASC";

            if(userRole.Name.Replace(" ","") != DataModels.Enums.UserRole.SuperAdmin.ToString() && userRole.Name.Replace(" ", "") != DataModels.Enums.UserRole.Admin.ToString())
            {
                datatableParams.UserId = userId;
            }

            List<ReservationDataVM> reservationsList = _reservationRepository.List(datatableParams);

            return reservationsList;
        }
    }
}
