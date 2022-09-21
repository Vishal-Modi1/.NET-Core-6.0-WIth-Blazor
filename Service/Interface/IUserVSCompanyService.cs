using DataModels.Entities;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface IUserVSCompanyService
    {
        UserVSCompany GetDefaultCompanyByUserId(long userId);
    }
}
