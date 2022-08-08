using DataModels.Entities;
using DataModels.VM.Common;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IUserVSCompanyRepository : IBaseRepository<UserVSCompany>
    {
        UserVSCompany GetDefaultCompanyByUserId(long userId);

        UserVSCompany Edit(UserVSCompany userVSCompany);

        List<UserVSCompany> ListByCompanyId(int companyId);
    }
}
