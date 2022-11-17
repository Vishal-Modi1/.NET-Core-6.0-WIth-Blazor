using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.User;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Service
{
    public class InviteUserService : BaseService, IInviteUserService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IInviteUserRepository _inviteUserRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailTokenRepository _emailTokenRepository;
        private readonly IUserVSCompanyRepository _userVSCompanyRepository;
        private readonly IUserRepository _userRepository;

        public InviteUserService(IUserRoleRepository userRoleRepository, IUserRepository userRepository,
            IInviteUserRepository inviteUserRepository, IUserVSCompanyRepository userVSCompanyRepository,
            ICompanyRepository companyRepository, IEmailTokenRepository emailTokenRepository)
        {
            _userRoleRepository = userRoleRepository;
            _inviteUserRepository = inviteUserRepository;
            _companyRepository = companyRepository;
            _emailTokenRepository = emailTokenRepository;
            _userVSCompanyRepository = userVSCompanyRepository;
            _userRepository = userRepository;
        }

        public CurrentResponse Create(InviteUserVM inviteUserVM)
        {
            try
            {
                InviteUser inviteUser = ToDataObject(inviteUserVM);
                inviteUser = _inviteUserRepository.Create(inviteUser);

                CreateResponse(inviteUser, System.Net.HttpStatusCode.OK, $"Invitation mail has been successfully sent on {inviteUserVM.Email}");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(int roleId, long id)
        {
            try
            {
                InviteUser inviteUser = _inviteUserRepository.GetById(id);
                InviteUserVM inviteUserVM = new InviteUserVM();

                if (inviteUser != null )
                {
                    inviteUserVM.CompanyId = inviteUser.CompanyId;
                    inviteUserVM.RoleId = inviteUser.RoleId;
                    inviteUserVM.Id = inviteUser.Id;
                    inviteUserVM.Email = inviteUser.Email;
                }
                
                inviteUserVM.ListRoles = _userRoleRepository.ListDropDownValues(roleId);

                if (roleId == (int)DataModels.Enums.UserRole.SuperAdmin)
                {
                    inviteUserVM.ListCompanies = _companyRepository.ListDropDownValues();
                }

                CreateResponse(inviteUserVM, System.Net.HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, System.Net.HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse IsValidInvite(InviteUserVM inviteUserVM)
        {
            try
            {
                bool isValid = false;

                List<InviteUser> listUserInvitations = _inviteUserRepository.SearchFor(p => p.CompanyId == inviteUserVM.CompanyId && p.Email == inviteUserVM.Email && p.IsDeleted == false);

                if (listUserInvitations.Count > 0)
                {
                    List<long> invitationIds = listUserInvitations.Select(p => p.Id).ToList();

                    isValid = _emailTokenRepository.FindByCondition(p => invitationIds.Contains(p.InvitedUserId.GetValueOrDefault()) && p.ExpireOn >= DateTime.UtcNow ) == null;

                    if (!isValid)
                    {
                        CreateResponse(isValid, System.Net.HttpStatusCode.Ambiguous, "User has been already invited. you can cannot invite same user twice in a day.");
                        return _currentResponse;
                    }
                }

                User user = _userRepository.FindByCondition(p => p.Email == inviteUserVM.Email);

                if (user != null)
                {
                    isValid = _userVSCompanyRepository.FindByCondition(p => p.CompanyId == inviteUserVM.CompanyId && p.UserId == user.Id) == null;
                    if (!isValid)
                    {
                        CreateResponse(isValid, System.Net.HttpStatusCode.Ambiguous, "User is already registered in your company.");
                        return _currentResponse;
                    }
                }
                

                CreateResponse(true, System.Net.HttpStatusCode.OK, "");
                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(false, System.Net.HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse AcceptInvitation(string token)
        {
            try
            {
                EmailToken emailToken = _emailTokenRepository.FindByCondition(p => p.Token == token);
                InviteUser inviteUser = _inviteUserRepository.FindByCondition(p => p.Id == emailToken.InvitedUserId);
                User user = _userRepository.FindByCondition(p => p.Email == inviteUser.Email);

                UserVSCompany userVSCompany = new UserVSCompany();
                userVSCompany.CompanyId = inviteUser.CompanyId;
                userVSCompany.RoleId = inviteUser.RoleId;
                userVSCompany.UserId = user.Id;
                userVSCompany.CreatedBy = user.Id;
                userVSCompany.CreatedOn = DateTime.UtcNow;
                userVSCompany.IsActive = true;

                _userVSCompanyRepository.Create(userVSCompany);

                _emailTokenRepository.UpdateStatus(token);

                CreateResponse(null, System.Net.HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, System.Net.HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(UserDatatableParams datatableParams)
        {
            try
            {
                List<InviteUserDataVM> invitedUsersList = _inviteUserRepository.List(datatableParams);
                CreateResponse(invitedUsersList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private InviteUser ToDataObject(InviteUserVM inviteUserVM)
        {
            InviteUser inviteUser = new InviteUser();

            inviteUser.Id = inviteUserVM.Id;
            inviteUser.CompanyId = inviteUserVM.CompanyId;
            inviteUser.Email = inviteUserVM.Email;
            inviteUser.RoleId = inviteUserVM.RoleId;
            inviteUser.InvitedBy = inviteUserVM.InvitedBy;
            inviteUser.InvitedOn = DateTime.UtcNow;

            return inviteUser;
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _inviteUserRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "User invitation deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(InviteUserVM inviteUserVM)
        {
            try
            {
                InviteUser inviteUser = ToDataObject(inviteUserVM);
                inviteUser = _inviteUserRepository.Edit(inviteUser);

                CreateResponse(inviteUser, HttpStatusCode.OK, $"Invitation details has been updated.");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }
    }
}
