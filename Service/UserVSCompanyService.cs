using Repository.Interface;
using Service.Interface;
using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Net;

namespace Service
{
    public class UserVSCompanyService : BaseService, IUserVSCompanyService
    {
        private readonly IUserVSCompanyRepository _userVSCompanyRepository;

        public UserVSCompanyService(IUserVSCompanyRepository userVSCompanyRepository)
        {
            _userVSCompanyRepository = userVSCompanyRepository;
        }

        //public CurrentResponse GetDefaultCompanyUserId(long userId)
        //{
        //    try
        //    {
        //        UserVSCompany userVSCompany = GetDefaultCompanyUserId(userId);

        //        CreateResponse(userVSCompany, HttpStatusCode.OK, "");

        //        return _currentResponse;
        //    }
        //    catch (Exception exc)
        //    {
        //        CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

        //        return _currentResponse;
        //    }
        //}

        public UserVSCompany GetDefaultCompanyByUserId(long userId)
        {
            return _userVSCompanyRepository.GetDefaultCompanyByUserId(userId);
        }
    }
}
