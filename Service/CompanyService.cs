using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Company;
using DataModels.VM.Common;
using Service.Interface;
using DataModels.Constants;

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
                    CreateResponse(companyVM, HttpStatusCode.Ambiguous, "Company is already exist");
                }
                else
                {
                    company = _companyRepository.Create(company);

                    companyVM = ToBusinessObject(company);

                    CreateResponse(companyVM, HttpStatusCode.OK, "Company added successfully");
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

                foreach (CompanyVM companyVM in companyList)
                {
                    companyVM.LogoPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectory.CompanyLogo}/{companyVM.Logo}";
                }

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

        public CurrentResponse ListDropDownValues()
        {
            try
            {
                List<DropDownValues> companiesList = _companyRepository.ListDropDownValues();

                CreateResponse(companiesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListCompanyServiceDropDownValues()
        {
            try
            {
                List<DropDownValues> companiesServicesList = _companyRepository.ListCompanyServicesDropDownValues();

                CreateResponse(companiesServicesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Delete(int id, long deletedBy)
        {
            try
            {
                _companyRepository.Delete(id, deletedBy);
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

        public CurrentResponse UpdateImageName(int id, string logoName)
        {
            try
            {
                bool isImageNameUpdated = _companyRepository.UpdateImageName(id, logoName);

                Company company = _companyRepository.FindByCondition(p => p.Id == id && p.IsDeleted != true && p.IsActive == true);
                company.Logo = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectory.CompanyLogo}/{company.Logo}";

                CreateResponse(company, HttpStatusCode.OK, "Company logo updated successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private bool IsCompanyExist(CompanyVM companyVM)
        {
            Company company = _companyRepository.FindByCondition(p => p.Name == companyVM.Name && p.Id != companyVM.Id);

            if (company == null)
            {
                return false;
            }

            return true;
        }

        #region Object Mapper

        private CompanyVM ToBusinessObject(Company company)
        {
            CompanyVM companyVM = new CompanyVM();

            companyVM.Id = company.Id;
            companyVM.Name = company.Name;
            companyVM.Address = company.Address;
            companyVM.ContactNo = company.ContactNo;
            companyVM.TimeZone = company.TimeZone;
            companyVM.Website = company.Website;
            companyVM.PrimaryAirport = company.PrimaryAirport;
            companyVM.PrimaryServiceId = company.PrimaryServiceId;
            companyVM.Logo = company.Logo;

            companyVM.LogoPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectory.CompanyLogo}/{companyVM.Logo}";

            return companyVM;
        }

        public Company ToDataObject(CompanyVM companyVM)
        {
            Company company = new Company();

            company.Id = companyVM.Id;
            company.Name = companyVM.Name;
            company.Address = companyVM.Address;
            company.ContactNo = companyVM.ContactNo;
            company.TimeZone = companyVM.TimeZone;
            company.Website = companyVM.Website;
            company.PrimaryAirport = companyVM.PrimaryAirport;
            company.PrimaryServiceId = companyVM.PrimaryServiceId == null ? null : (short)companyVM.PrimaryServiceId;

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

        #endregion
    }
}
