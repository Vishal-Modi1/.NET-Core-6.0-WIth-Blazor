using DataModels.Entities;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;
using DataModels.VM.Account;
using DataModels.VM.Common;
using DataModels.Constants;

namespace Service
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserVSCompanyRepository _userVSCompanyRepository;

        public AccountService(IAccountRepository accountRepository, ICompanyRepository companyRepository, 
            IUserRoleRepository userRoleRepository, IUserVSCompanyRepository userVSCompanyRepository)
        {
            _accountRepository = accountRepository;
            _companyRepository = companyRepository; 
            _userRoleRepository = userRoleRepository;
            _userVSCompanyRepository = userVSCompanyRepository;
        }

        public CurrentResponse GetValidUser(LoginVM loginVM)
        {
            try
            {
                User user = _accountRepository.GetValidUser(loginVM.Email, loginVM.Password);

                if (user == null)
                {
                    CreateResponse(null, HttpStatusCode.Unauthorized, "Invalid Credentials");
                    
                    return _currentResponse;
                }

                UserVSCompany userVSCompany = _userVSCompanyRepository.GetDefaultCompanyByUserId(user.Id);
                string companyId = "0";

                if(userVSCompany != null)
                {
                    companyId = userVSCompany.CompanyId.GetValueOrDefault().ToString();
                    user.CompanyId = userVSCompany.CompanyId;
                }

                user.ImageName = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.UserProfileImage}/{companyId}/{user.ImageName}";
                Company company = _companyRepository.FindByCondition(p => p.Id == user.CompanyId);

                if(company != null)
                {
                    if(!company.IsActive)
                    {
                        CreateResponse(null, HttpStatusCode.NotFound, "Your organisation is not activated");

                        return _currentResponse;
                    }

                    if(company.IsDeleted)
                    {
                        CreateResponse(null, HttpStatusCode.NotFound, "Organization has been deleted");

                        return _currentResponse;
                    }
                }
               
                if (!user.IsActive)
                {
                    CreateResponse(null, HttpStatusCode.NotFound, "Your account is not activated");
                }
                else if (user.IsDeleted)
                {
                    CreateResponse(null, HttpStatusCode.NotFound, "Your account has been deleted");
                }
                else
                {
                    if (company != null)
                    {
                        user.CompanyName = company.Name;
                    }

                    var userRoleDetails = _userRoleRepository.FindByUserIdAndCompanyId(user.Id, user.CompanyId);

                    user.RoleName = userRoleDetails.Name;
                    user.RoleId = userRoleDetails.Id;

                    CreateResponse(user, HttpStatusCode.OK, "User is valid");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());
                return _currentResponse;
            }
        }

        public CurrentResponse ActivateAccount(string token)
        {
            bool isActivated = _accountRepository.ActivateAccount(token);

            if (isActivated)
            {
                CreateResponse(isActivated, HttpStatusCode.OK, "Account activated");
            }
            else
            {
                CreateResponse(isActivated, HttpStatusCode.OK, "Account activation failed");
            }

            return _currentResponse;
        }
    }
}
