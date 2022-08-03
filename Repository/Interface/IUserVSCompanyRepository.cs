using DataModels.Entities;

namespace Repository.Interface
{
    public interface IUserVSCompanyRepository : IBaseRepository<UserVSCompany>
    {
        UserVSCompany GetDefaultCompanyByUserId(long userId);

        UserVSCompany Edit(UserVSCompany userVSCompany);
    }
}
