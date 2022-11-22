using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using System.Collections.Generic;

namespace Repository.Interface
{
    public interface IEmailConfigurationRepository : IBaseRepository<EmailConfiguration>
    {
        EmailConfiguration Edit(EmailConfiguration discrepancy);

        List<EmailConfigurationDataVM> List(DatatableParams datatableParams);

        List<DropDownValues> ListDropdownEmailTypes();
    }
}
