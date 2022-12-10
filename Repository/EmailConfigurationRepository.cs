using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class EmailConfigurationRepository : BaseRepository<EmailConfiguration>, IEmailConfigurationRepository
    {
        private readonly MyContext _myContext;

        public EmailConfigurationRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public EmailConfiguration Edit(EmailConfiguration emailConfiguration)
        {
            EmailConfiguration existingEmailConfiguration = _myContext.EmailsConfiguration.Where(p => p.Id == emailConfiguration.Id).FirstOrDefault();

            if (existingEmailConfiguration != null)
            {
                existingEmailConfiguration.EmailTypeId = emailConfiguration.EmailTypeId;
                existingEmailConfiguration.Email = emailConfiguration.Email;
                existingEmailConfiguration.CompanyId = emailConfiguration.CompanyId;

                _myContext.SaveChanges();
            }

            return emailConfiguration;
        }

        public List<EmailConfigurationDataVM> List(DatatableParams datatableParams)
        {
            List<EmailConfigurationDataVM> list;

            string sql = $"EXEC dbo.GetEmailsConfigurationList '{datatableParams.SearchText }', { datatableParams.Start }, " +
                $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', " +
                $"{datatableParams.CompanyId}";

            list = _myContext.EmailsConfigurationDataVM.FromSqlRaw<EmailConfigurationDataVM>(sql).ToList();

            return list;
        }

        public List<DropDownValues> ListDropdownEmailTypes()
        {
            List<DropDownValues> values = _myContext.EmailTypes.ToList().Select(p => new DropDownValues()
                                           {
                                               Id = p.Id,
                                               Name = p.Name,
                                           }).ToList();

            return values;
        }
    }
}
