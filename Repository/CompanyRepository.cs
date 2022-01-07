using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataModels.VM.Company;
using DataModels.VM.Common;

namespace Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private MyContext _myContext;

        public List<CompanyVM> ListAll()
        {
            using (_myContext = new MyContext())
            {
                List<CompanyVM> companyDataList = _myContext.Companies.Where(p => p.IsDeleted == false).Select(n => new CompanyVM { Id = n.Id, Name = n.Name }).ToList();

                return companyDataList;
            
            }
        }

        public List<DropDownValues> ListDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> companyList = (from company in _myContext.Companies
                                                     where company.IsDeleted == false
                                                     select new DropDownValues()
                                                     {
                                                         Id = company.Id,
                                                         Name = company.Name
                                                     }).ToList();

                return companyList;
            }
        }

        public Company Create(Company company)
        {
            using (_myContext = new MyContext())
            {
                _myContext.Companies.Add(company);
                _myContext.SaveChanges();

                return company;
            }
        }

        public Company Edit(Company company)
        {
            using (_myContext = new MyContext())
            {
                Company existingCompany = _myContext.Companies.Where(p => p.Id == company.Id).FirstOrDefault();

                if (existingCompany != null)
                {
                    existingCompany.Name = company.Name;
                    existingCompany.Address = company.Address;
                    existingCompany.ContactNo = company.ContactNo;
                }

                _myContext.SaveChanges();

                return company;
            }
        }

        public void Delete(int id)
        {
            using (_myContext = new MyContext())
            {
                Company company = _myContext.Companies.Where(p => p.Id == id).FirstOrDefault();

                if (company != null)
                {
                    company.IsDeleted = true;
                    _myContext.SaveChanges();
                }
            }
        }

        public List<CompanyVM> List(DatatableParams datatableParams)
        {
            using (_myContext = new MyContext())
            {
                int pageNo = datatableParams.Start >= 10 ? ((datatableParams.Start / datatableParams.Length) + 1) : 1;
                List<CompanyVM> list;

                string sql = $"EXEC dbo.GetCompanyList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'";

                list = _myContext.CompanyData.FromSqlRaw<CompanyVM>(sql).ToList();

                return list;
            }
        }

        public Company FindByCondition(Expression<Func<Company, bool>> predicate)
        {
            using (_myContext = new MyContext())
            {
                return _myContext.Companies.Where(predicate).FirstOrDefault();
            }
        }
    }
}
