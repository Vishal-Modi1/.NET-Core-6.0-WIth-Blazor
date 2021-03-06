using DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataModels.VM.Company;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface ICompanyRepository
    {
        List<CompanyVM> ListAll();

        Company Create(Company company);

        List<CompanyVM> List(DatatableParams datatableParams);

        Company Edit(Company company);

        Company FindByCondition(Expression<Func<Company, bool>> predicate);

        void Delete(int id, long deletedBy);

        List<DropDownValues> ListDropDownValues();

        List<DropDownValues> ListCompanyServicesDropDownValues();

        bool UpdateImageName(int id, string logoName);
    }
}
