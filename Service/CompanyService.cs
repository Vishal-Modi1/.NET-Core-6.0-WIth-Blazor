using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Company;
using DataModels.VM.Common;
using Service.Interface;

namespace Service
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public CurrentResponse Create(CompanyVM companyVM)
        {
            Company company = ToDataObject(companyVM);

            try
            {
                bool isCompanyExist = IsCompanyExist(companyVM);

                if (isCompanyExist)
                {
                    CreateResponse(company, HttpStatusCode.Ambiguous, "Company is already exist");
                }
                else
                {
                    company = _companyRepository.Create(company);
                    CreateResponse(company, HttpStatusCode.OK, "Company added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(CompanyVM companyVM)
        {
            Company company = ToDataObject(companyVM);

            try
            {
                bool isCompanyExist = IsCompanyExist(companyVM);

                if (isCompanyExist)
                {
                    CreateResponse(company, HttpStatusCode.Ambiguous, "Company is already exist");
                }

                else
                {
                    company = _companyRepository.Edit(company);
                    CreateResponse(company, HttpStatusCode.OK, "Company updated successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(DatatableParams datatableParams)
        {
            try
            {
                List<CompanyVM> companyList = _companyRepository.List(datatableParams);

                CreateResponse(companyList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListAll()
        {
            try
            {
                List<CompanyVM> companyList = _companyRepository.ListAll();

                CreateResponse(companyList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id)
        {
            try
            {
                _companyRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Company deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(int id)
        {
            Company company = _companyRepository.FindByCondition(p => p.Id == id);
            CompanyVM companyVM = new CompanyVM();

            if (companyVM != null)
            {
                companyVM = ToBusinessObject(company);
            }

            CreateResponse(companyVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }

        private CompanyVM ToBusinessObject(Company company)
        {
            CompanyVM companyVM = new CompanyVM();

            companyVM.Id = company.Id;
            companyVM.Name = company.Name;
            companyVM.Address = company.Address;
            companyVM.ContactNo = company.ContactNo;

            return companyVM;
        }

        public Company ToDataObject(CompanyVM companyVM)
        {
            Company company = new Company();

            company.Id = companyVM.Id;
            company.Name = companyVM.Name;
            company.Address = companyVM.Address;
            company.ContactNo = companyVM.ContactNo;
            company.CreatedBy = companyVM.CreatedBy;
            company.UpdatedBy = companyVM.UpdatedBy;
            company.IsActive = true;

            if (companyVM.Id == 0)
            {
                company.CreatedOn = DateTime.UtcNow;
            }

            company.UpdatedOn = DateTime.UtcNow;

            return company;
        }

        private bool IsCompanyExist(CompanyVM companyVM)
        {
            Company company = _companyRepository.FindByCondition(p => p.Name == companyVM.Name && p.Id != companyVM.Id);

            if(company == null)
            {
                return false;
            }

            return true;
        }
    }
}
