using DataModels.Entities;
using DataModels.VM.Common;
using System;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface ICompanyDateFormatRepository : IBaseRepository<CompanyDateFormat>
    {
        CompanyDateFormat SetDefault(CompanyDateFormat companyDateFormat);
        List<DropDownSmallValues> ListDropDownValues();
        string FindDateFormatValue(int companyId);
    }
}
