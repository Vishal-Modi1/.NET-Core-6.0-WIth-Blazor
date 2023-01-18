using DataModels.VM.Company;
using DataModels.VM.Common;
using DataModels.Entities;
using System.Linq.Expressions;
using System;

namespace Service.Interface
{
    public interface ICompanyService
    {
        CurrentResponse Create(CompanyVM companyVM);
        CurrentResponse List(CompanyDatatableParams datatableParams);
        CurrentResponse ListDropDownValues();
        CurrentResponse ListAll();
        CurrentResponse Edit(CompanyVM companyVM);
        CurrentResponse FindById(int id);
        CurrentResponse Delete(int id, long deletedBy);
        CurrentResponse ListCompanyServiceDropDownValues();
        CurrentResponse UpdateImageName(int id, string logoName);
        CurrentResponse UpdateCreatedBy(int id, long createdBy);
        CurrentResponse IsCompanyExist(int id, string name);
        CurrentResponse ListDropDownValuesByUserId(long userId);
        Company FindByCondition(Expression<Func<Company, bool>> predicate);
        CurrentResponse GetFiltersValue();
        CurrentResponse SetPropellerConfiguration(int id, bool value);

        CurrentResponse IsDisplayPropeller(int id);
    }
}
