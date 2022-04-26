using DataModels.VM.Company;
using DataModels.VM.Common;

namespace Service.Interface
{
    public interface ICompanyService
    {
        CurrentResponse Create(CompanyVM companyVM);
        CurrentResponse List(DatatableParams datatableParams);
        CurrentResponse ListDropDownValues();
        CurrentResponse ListAll();
        CurrentResponse Edit(CompanyVM companyVM);
        CurrentResponse GetDetails(int id);
        CurrentResponse Delete(int id, long deletedBy);
        CurrentResponse ListCompanyServiceDropDownValues();
        CurrentResponse UpdateImageName(int id, string logoName);
    }
}
