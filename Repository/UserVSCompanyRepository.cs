using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class UserVSCompanyRepository : BaseRepository<UserVSCompany>, IUserVSCompanyRepository
    {
        private readonly MyContext _myContext;

        public UserVSCompanyRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public UserVSCompany Edit(UserVSCompany userVSCompany)
        {

            UserVSCompany existingDetails = _myContext.UsersVsCompanies.Where(x => x.UserId == userVSCompany.UserId && x.CompanyId == userVSCompany.ExistingCompanyId).FirstOrDefault();

            if (existingDetails != null)
            {
                existingDetails.CompanyId = userVSCompany.CompanyId;
                existingDetails.RoleId = userVSCompany.RoleId;
                existingDetails.UpdatedOn = System.DateTime.UtcNow;
                existingDetails.UpdatedBy = userVSCompany.UpdatedBy;

                _myContext.SaveChanges();
            }

            return userVSCompany;
        }

        public UserVSCompany GetDefaultCompanyByUserId(long userId)
        {
            return _myContext.UsersVsCompanies.Where(p => p.UserId == userId && p.IsActive && !p.IsDeleted).FirstOrDefault();
        }

        public List<UserVSCompany> ListByCompanyId(int companyId)
        {
            List<UserVSCompany> userRolesList = _myContext.UsersVsCompanies.Where(p=>p.CompanyId == companyId && p.IsActive == true && p.IsDeleted == false).ToList();

            return userRolesList;
        }
    }
}
