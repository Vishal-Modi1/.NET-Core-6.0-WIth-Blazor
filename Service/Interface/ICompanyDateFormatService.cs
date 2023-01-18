using DataModels.VM.Common;
using DataModels.VM.Company.Settings;

namespace Service.Interface
{
    public interface ICompanyDateFormatService
    {
        CurrentResponse FindByCompanyId(long companyId);

        CurrentResponse SetDefault(CompanyDateFormatVM CompanyDateFormatVM);

        CurrentResponse ListDropDownValues();

        string FindDateFormatByCompanyId(int companyId);
    }
}
