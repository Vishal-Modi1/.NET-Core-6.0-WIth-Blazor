using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using DataModels.VM.Common;
using DataModels.VM.User;
using DataModels.VM.Account;
using DataModels.Constants;
using System.Collections.Generic;
using DataModels.VM.UserPreference;

namespace Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IInstructorTypeRepository _instructorTypeRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserVSCompanyRepository _userVSCompanyRepository;
        private readonly IInviteUserRepository _inviteUserRepository;
        private readonly IEmailTokenRepository _emailTokenRepository;

        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository
            , IInstructorTypeRepository instructorTypeRepository, ICountryRepository countryRepository,
            ICompanyRepository companyRepository, IUserVSCompanyRepository userVSCompanyRepository,
            IInviteUserRepository inviteUserRepository, IEmailTokenRepository emailTokenRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _instructorTypeRepository = instructorTypeRepository;
            _countryRepository = countryRepository;
            _companyRepository = companyRepository;
            _userVSCompanyRepository = userVSCompanyRepository;
            _inviteUserRepository = inviteUserRepository;
            _emailTokenRepository = emailTokenRepository;
        }

        public CurrentResponse ListDropdownValuesByCompanyId(int companyId)
        {
            try
            {
                List<DropDownLargeValues> usersList = _userRepository.ListDropdownValuesbyCompanyId(companyId);
                CreateResponse(usersList, HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());
            }

            return _currentResponse;
        }

        public CurrentResponse GetDetails(long id, int companyId, int roleId)
        {
            UserVM userVM = GetUserDetails(id, companyId, roleId);

            CreateResponse(userVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }

        public UserVM GetUserDetails(long id, int companyId, int roleId)
        {
            User user = _userRepository.FindByCondition(p => p.Id == id && p.IsDeleted != true);

            UserVM userVM = new UserVM();

            if (user != null)
            {
                userVM = ToBusinessObject(user);
            }

            GetMasterDetails(userVM, roleId);

            if (companyId > 0)
            {
                userVM.CompanyId = companyId;
                userVM.CompanyName = _companyRepository.FindByCondition(p => p.Id == companyId).Name;
            }
            else
            {
                userVM.Companies = _companyRepository.ListDropDownValues();
            }

            if (userVM.Id > 0)
            {
                var userCompanyDetails = _userVSCompanyRepository.FindByCondition(p => p.UserId == userVM.Id && p.CompanyId == userVM.CompanyId);
                var userRole = _userRoleRepository.FindById(userCompanyDetails.RoleId);

                userVM.RoleId = userRole.Id;
                userVM.Role = userRole.Name;
            }

            return userVM;
        }

        private void GetMasterDetails(UserVM userVM, int roleId)
        {
            userVM.Countries = _countryRepository.ListDropDownValues();
            userVM.InstructorTypes = _instructorTypeRepository.ListDropDownValues();
            userVM.UserRoles = _userRoleRepository.ListDropDownValues(roleId);
        }

        public CurrentResponse GetMasterDetails(int roleId, bool isInvited, string token)
        {
            try
            {
                UserVM userVM = new UserVM();

                if (isInvited)
                {
                    EmailToken emailToken = _emailTokenRepository.FindByCondition(p => p.Token == token);
                    InviteUser inviteUser = _inviteUserRepository.GetById(emailToken.InvitedUserId.GetValueOrDefault());

                    userVM.CompanyId = inviteUser.CompanyId;
                    userVM.Email = inviteUser.Email;

                    bool isEmailExist = _userRepository.IsEmailExist(userVM.Email);

                    if(isEmailExist)
                    {
                        bool isSuperAdmin = roleId == (int)DataModels.Enums.UserRole.SuperAdmin;
                        User user = _userRepository.FindByCondition(p=>p.Email == userVM.Email);

                        userVM = _userRepository.FindById(user.Id, isSuperAdmin, isInvited, userVM.CompanyId);
                        
                    }

                    userVM.IsInvited = isInvited;

                    userVM.Role = _userRoleRepository.FindById(inviteUser.RoleId).Name;
                    userVM.RoleId = inviteUser.RoleId;
                }

                GetMasterDetails(userVM , roleId);

                CreateResponse(userVM, HttpStatusCode.OK, "Master details retrived successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Create(UserVM userVM)
        {
            try
            {
                User user = ToDataObject(userVM);
                user = _userRepository.Create(user);

                userVM.Id = user.Id;

                SaveUserVSCompany(userVM);
                CreateResponse(user, HttpStatusCode.OK, "User added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private void SaveUserVSCompany(UserVM userVM)
        {
            UserVSCompany userVSCompany = new UserVSCompany();
            userVSCompany.CompanyId = userVM.CompanyId;
            userVSCompany.RoleId = userVM.RoleId;
            userVSCompany.UserId = userVM.Id;
            
            userVSCompany.CreatedOn = DateTime.UtcNow;
            userVSCompany.CreatedBy = userVM.Id;
            userVSCompany.IsActive = true;
            userVSCompany.IsDeleted = false;

            _userVSCompanyRepository.Create(userVSCompany);
        }

        public CurrentResponse Edit(UserVM userVM)
        {
            try
            {
                User user = ToDataObject(userVM);
                user = _userRepository.Edit(user);

                var userCompanyDetails = _userVSCompanyRepository.FindByCondition(p => p.UserId == userVM.Id && userVM.CompanyId == p.CompanyId); 

                if(userCompanyDetails != null)
                {
                    userCompanyDetails.UpdatedBy = userVM.UpdatedBy;
                    userCompanyDetails.CompanyId = userVM.CompanyId;
                    userCompanyDetails.RoleId = userVM.RoleId;

                    _userVSCompanyRepository.Edit(userCompanyDetails);
                }

                CreateResponse(user, HttpStatusCode.OK, "User updated successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse IsEmailExist(string email)
        {
            try
            {
                bool isEmailExist = _userRepository.IsEmailExist(email);

                CreateResponse(isEmailExist, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            try
            {
                bool isValidToken = _emailTokenRepository.IsValidToken(resetPasswordVM.Token);

                if (!isValidToken)
                {
                    return CreateResponse(isValidToken, HttpStatusCode.NotFound, "Token is expired");
                }

                bool isUserPasswordReset = _userRepository.ResetUserPassword(resetPasswordVM);

                if (isUserPasswordReset)
                {
                    _emailTokenRepository.UpdateStatus(resetPasswordVM.Token);

                    CreateResponse(isUserPasswordReset, HttpStatusCode.OK, "Password reset successfully");
                }
                else
                {
                    CreateResponse(isUserPasswordReset, HttpStatusCode.OK, "Something went wrong. Try again later.");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(UserDatatableParams datatableParams)
        {
            try
            {
                List<UserDataVM> usersList = _userRepository.List(datatableParams);

                foreach (UserDataVM user in usersList)
                {
                    //if(!string.IsNullOrWhiteSpace(user.ProfileImage))
                   // {
                        user.ProfileImage = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.UserProfileImage}/{user.CompanyId.GetValueOrDefault()}/{user.ProfileImage}";
                        
                   // }
                }

                CreateResponse(usersList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(long id, long deletedBy)
        {
            try
            {
                _userRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "User deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateActiveStatus(long id, bool isActive)
        {
            try
            {
                _userRepository.UpdateActiveStatus(id, isActive);

                string message = "User activated successfully.";

                if (!isActive)
                {
                    message = "User deactivated successfully.";

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
                UserFilterVM userFilterVM = new UserFilterVM();

                userFilterVM.Companies = _companyRepository.ListDropDownValues();
                userFilterVM.UserRoles = _userRoleRepository.ListDropDownValues(roleId);

                CreateResponse(userFilterVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new UserFilterVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindById(long id, bool isSuperAdmin, int? companyId)
        {
            try
            {
                UserVM userVM = _userRepository.FindById(id, isSuperAdmin,false, companyId);

                string company = "0";

                if (!string.IsNullOrWhiteSpace(userVM.CompanyId.ToString()))
                {
                    company = userVM.CompanyId.ToString();
                }

                var userRole = _userRoleRepository.FindByUserIdAndCompanyId(userVM.Id, userVM.CompanyId);

                userVM.Role = userRole.Name;
                userVM.RoleId = userRole.Id;

                userVM.ImageName = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.UserProfileImage}/{company}/{userVM.ImageName}";

                userVM.Countries = _countryRepository.ListDropDownValues();
                userVM.InstructorTypes = _instructorTypeRepository.ListDropDownValues();
                userVM.UserRoles = _userRoleRepository.ListDropDownValues(userVM.RoleId);

                CreateResponse(userVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new UserVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateImageName(long id, string imageName)
        {
            try
            {
                bool isImageNameUpdated = _userRepository.UpdateImageName(id, imageName);

                User user = _userRepository.FindByCondition(p => p.Id == id && p.IsDeleted != true && p.IsActive == true);
                user.ImageName = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.UserProfileImage}/{user}/{user.ImageName}";

                CreateResponse(user, HttpStatusCode.OK, "Profile picture updated successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindMyPreferencesById(long id)
        {
            try
            {
                List<UserPreferenceVM> listUserPreferences = _userRepository.FindPreferenceById(id);

                CreateResponse(listUserPreferences, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new UserVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetById(long id, int companyId)
        {
            try
            {
                User user = _userRepository.FindByCondition(p => p.Id == id);

                user.CompanyId = companyId;
                user.ImageName = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.UserProfileImage}/{companyId}/{user.ImageName}";
                Company company = _companyRepository.FindByCondition(p => p.Id == user.CompanyId);

                if (company != null)
                {
                    user.CompanyName = company.Name;
                }

                var userRole = _userRoleRepository.FindByUserIdAndCompanyId(user.Id, user.CompanyId);

                user.RoleName = userRole.Name;
                user.RoleId = userRole.Id;

                CreateResponse(user, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new UserVM(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #region Object Mapping

        public User ToDataObject(UserVM userVM)
        {
            User user = new User();

            user.Id = userVM.Id;
            user.CompanyId = userVM.CompanyId;
            user.DateofBirth = userVM.DateofBirth;
            user.Email = userVM.Email;
            user.FirstName = userVM.FirstName;
            user.LastName = userVM.LastName;

            if (!string.IsNullOrWhiteSpace(userVM.Password))
            {
                user.Password = userVM.Password;
            }

            user.Phone = userVM.Phone;

            user.RoleId = userVM.RoleId;
            user.IsInstructor = userVM.IsInstructor;
            user.InstructorTypeId = (bool)userVM.IsInstructor ? userVM.InstructorTypeId : null;
            user.Gender = userVM.Gender;
            user.ExternalId = userVM.ExternalId;
            user.CountryId = userVM.CountryId;
            user.ExternalId = userVM.ExternalId;
            user.IsSendEmailInvite = userVM.IsSendEmailInvite;
            user.IsSendTextMessage = userVM.IsSendTextMessage;
            user.CreatedBy = userVM.CreatedBy;
            user.UpdatedBy = userVM.UpdatedBy;

            user.Gender = userVM.Gender;

            if (userVM.Id == 0)
            {
                user.CreatedOn = DateTime.UtcNow;
            }

            user.UpdatedOn = DateTime.UtcNow;

            return user;
        }

        public UserVM ToBusinessObject(User user)
        {
            UserVM userDetails = new UserVM();

            userDetails.Id = user.Id;
            userDetails.CompanyId = user.CompanyId;
            userDetails.DateofBirth = user.DateofBirth;
            userDetails.Email = user.Email;
            userDetails.FirstName = user.FirstName;
            userDetails.LastName = user.LastName;
            userDetails.Password = user.Password;
            userDetails.Phone = user.Phone;
            userDetails.RoleId = user.RoleId;
            userDetails.IsInstructor = user.IsInstructor;
            userDetails.InstructorTypeId = user.InstructorTypeId == null ? 0 : (int)user.InstructorTypeId;
            userDetails.Gender = user.Gender;
            userDetails.ExternalId = user.ExternalId;
            userDetails.CountryId = user.CountryId;
            userDetails.ExternalId = user.ExternalId;
            userDetails.IsSendEmailInvite = user.IsSendEmailInvite;
            userDetails.Gender = user.Gender;
            userDetails.IsSendTextMessage = user.IsSendTextMessage;

            return userDetails;
        }

        #endregion
    }
}
