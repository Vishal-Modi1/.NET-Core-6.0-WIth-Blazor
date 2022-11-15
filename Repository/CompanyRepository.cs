using DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using DataModels.VM.Company;
using DataModels.VM.Common;

namespace Repository
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly MyContext _myContext;

        public CompanyRepository(MyContext dbContext)
            : base(dbContext)
        {
            this._myContext = dbContext;
        }

        public List<CompanyVM> ListAll()
        {

            List<CompanyVM> companyDataList = _myContext.Companies.Where(p => p.IsDeleted == false).
                                               Select(n => new CompanyVM
                                               { Id = n.Id, Name = n.Name }).ToList();

            return companyDataList;

        }

        public List<DropDownValues> ListDropDownValues()
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

        public List<DropDownValues> ListCityDropDownValues()
        {
            var data = (from company in _myContext.Companies
                        group company by company.City into c
                        select new DropDownValues()
                        {
                            Id = 1,
                            Name = c.First().City
                        }).ToList();

            data = data.Where(p => p.Name != "").ToList();

            return data;
        }

        public List<DropDownValues> ListStateDropDownValues()
        {
            List<DropDownValues> data = (from company in _myContext.Companies
                        group company by company.State into c
                        select new DropDownValues()
                        {
                            Id = 1,
                            Name = c.First().State
                        }).ToList();

            data = data.Where(p => p.Name != "").ToList();

            return data;
        }

        public List<DropDownValues> ListDropDownValuesByUserId(long userId)
        {
            List<DropDownValues> companyList = (from company in _myContext.Companies
                                                join usercompany in _myContext.UsersVsCompanies
                                                on company.Id equals usercompany.CompanyId
                                                where company.IsDeleted == false &&
                                                usercompany.IsActive == true &&
                                                usercompany.IsDeleted == false 
                                                && usercompany.UserId == userId
                                                select new DropDownValues()
                                                {
                                                    Id = company.Id,
                                                    Name = company.Name
                                                }).ToList();

            return companyList;
        }

        public List<DropDownValues> ListCompanyServicesDropDownValues()
        {
            List<DropDownValues> companyServices = (from companyService in _myContext.CompanyServices
                                                    select new DropDownValues()
                                                    {
                                                        Id = (int)companyService.Id,
                                                        Name = companyService.Name
                                                    }).ToList();

            return companyServices;
        }

        //public Company Create(Company company)
        //{
        //    {
        //        _myContext.Companies.Add(company);
        //        _myContext.SaveChanges();

        //        return company;
        //}

        public Company Edit(Company company)
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
                existingCompany.State = company.State;
                existingCompany.City = company.City;
                existingCompany.Zipcode = company.Zipcode;

                existingCompany.UpdatedBy = company.UpdatedBy;
                existingCompany.UpdatedOn = company.UpdatedOn;

                _myContext.SaveChanges();
            }

            return company;
        }

        public void Delete(int id, long deletedBy)
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

        public List<CompanyVM> List(CompanyDatatableParams datatableParams)
        {
            List<CompanyVM> list;

            string sql = $"EXEC dbo.GetCompaniesList '{ datatableParams.SearchText }', { datatableParams.Start }, " +
                $"{datatableParams.Length},'{datatableParams.SortOrderColumn}','{datatableParams.OrderType}'," +
                $" '{datatableParams.City}','{datatableParams.State}',{datatableParams.CompanyId}";

            list = _myContext.CompanyData.FromSqlRaw<CompanyVM>(sql).ToList();

            return list;
        }

        public bool UpdateImageName(int id, string logoName)
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
