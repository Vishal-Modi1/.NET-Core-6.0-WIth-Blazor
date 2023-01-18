using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;

namespace Service.Interface
{
    public interface IEmailConfigurationService
    {
        CurrentResponse Create(EmailConfigurationVM emailConfigurationVM);
        CurrentResponse Edit(EmailConfigurationVM emailConfigurationVM);
        CurrentResponse List(DatatableParams datatableParams);
        CurrentResponse GetDetails(long id);
        CurrentResponse GetDetailsByEmailTypeAndCompanyId(int emailTypeId, int companyId);
        //EmailConfiguration FindByCondition(Expression<Func<EmailConfiguration, bool>> predicate);
    }
}
