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
                List<CompanyVM> companyDataList = _myContext.Companies.Where(p => p.IsDeleted == false).
                                                   Select(n => new CompanyVM 
                                                   { Id = n.Id, Name = n.Name }).ToList();

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

        public List<DropDownValues> ListCompanyServicesDropDownValues()
        {
            using (_myContext = new MyContext())
            {
                List<DropDownValues> companyServices = (from companyService in _myContext.CompanyServices
                                                    select new DropDownValues()
                                                    {
                                                        Id = (int)companyService.Id,
                                                        Name = companyService.Name
                                                    }).ToList();

                return companyServices;
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
                    existingCompany.TimeZone = company.TimeZone;
                    existingCompany.Website = company.Website;
                    existingCompany.PrimaryAirport = company.PrimaryAirport;
                    existingCompany.PrimaryServiceId = company.PrimaryServiceId;

                    existingCompany.UpdatedBy = company.UpdatedBy;
                    existingCompany.UpdatedOn = company.UpdatedOn;
                }

                _myContext.SaveChanges();

                return company;
            }
        }

        public void Delete(int id, long deletedBy)
        {
            using (_myContext = new MyContext())
            {
                Company company = _myContext.Companies.Where(p => p.Id == id).FirstOrDefault();

                if (company != null)
                {
                    company.IsDeleted = true;
                    company.DeletedBy = deletedBy;
                    company.DeletedOn = DateTime.UtcNow;

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

                string sql = $"EXEC dbo.GetCompanyList '{ datatableParams.SearchText }', { pageNo }, {datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}', {datatableParams.CompanyId}";

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

        public bool UpdateImageName(int id, string logoName)
        {
            using (_myContext = new MyContext())
            {
                Company existingCompany = _myContext.Companies.Where(p => p.Id == id).FirstOrDefault();

                if (existingCompany != null)
                {
                    existingCompany.Logo = logoName;
                    _myContext.SaveChanges();

                    return true;
                }

                return false;
            }
        }
    }
}
