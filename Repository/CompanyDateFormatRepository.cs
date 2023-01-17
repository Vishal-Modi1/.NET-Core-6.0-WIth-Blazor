using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class CompanyDateFormatRepository : BaseRepository<CompanyDateFormat>, ICompanyDateFormatRepository
    {
        private readonly MyContext _myContext; 
        
        public CompanyDateFormatRepository(MyContext dbContext) : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public List<DropDownSmallValues> ListDropDownValues()
        {
            List<DropDownSmallValues> dateFormats = (from dateFormat in _myContext.DateFormats
                                                select new DropDownSmallValues() {
                                                
                                                    Id = dateFormat.Id,
                                                    Name = dateFormat.Name,

                                                }).ToList();
        
            return dateFormats;
        }

        public CompanyDateFormat SetDefault(CompanyDateFormat companyDateFormat)
        {
            CompanyDateFormat existingValue = _myContext.CompaniesDateFormat.Where(p=>p.Id == companyDateFormat.Id).FirstOrDefault();  
            
            if(existingValue != null)
            {
                existingValue.CompanyId = companyDateFormat.CompanyId;
                existingValue.DateFormatId = companyDateFormat.DateFormatId;
            }
            else
            {
                _myContext.CompaniesDateFormat.Add(companyDateFormat);
            }

            _myContext.SaveChanges();

            return companyDateFormat;
        }

        public string FindDateFormatValue(int companyId)
        {
            string dateFormatValue = (from dateFormat in _myContext.DateFormats 
                        join companyDateFormat in _myContext.CompaniesDateFormat
                        on dateFormat.Id equals companyDateFormat.DateFormatId 
                        where companyDateFormat.CompanyId == companyId select dateFormat.Name).FirstOrDefault();

            return dateFormatValue;
        }
    }
}
