﻿using DataModels.Entities;
using System.Collections.Generic;
using DataModels.VM.Company;
using DataModels.VM.Common;

namespace Repository.Interface
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        List<CompanyVM> ListAll();

        List<CompanyVM> List(DatatableParams datatableParams);

        Company Edit(Company company);

        void Delete(int id, long deletedBy);

        List<DropDownValues> ListDropDownValues();

        List<DropDownValues> ListCompanyServicesDropDownValues();

        bool UpdateImageName(int id, string logoName);

        List<DropDownValues> ListDropDownValuesByUserId(long userId);
    }
}
