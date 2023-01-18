using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Company;
using DataModels.VM.Common;
using Service.Interface;
using DataModels.Constants;
using System.Linq.Expressions;

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
                    companyVM = ToBusinessObject(company);

                    CreateResponse(companyVM, HttpStatusCode.OK, "Company updated successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetFiltersValue()
        {
            try
            {
                CompanyFilter companyFilter = new CompanyFilter();

                companyFilter.Cities = _companyRepository.ListCityDropDownValues();
                companyFilter.States = _companyRepository.ListStateDropDownValues();

                CreateResponse(companyFilter, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(new CompanyFilter(), HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(CompanyDatatableParams datatableParams)
        {
            try
            {
                List<CompanyVM> companyList = _companyRepository.List(datatableParams);

                foreach (CompanyVM companyVM in companyList)
                {
                    companyVM.LogoPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.CompanyLogo}/{companyVM.Logo}";
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

        public CurrentResponse ListDropDownValuesByUserId(long userId)
        {
            try
            {
                List<DropDownValues> companiesList = _companyRepository.ListDropDownValuesByUserId(userId);

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

        public CurrentResponse SetPropellerConfiguration(int id, bool value)
        {
            try
            {
                _companyRepository.SetPropellerConfiguration(id, value);
                CreateResponse(true, HttpStatusCode.OK, "Value updated");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse FindById(int id)
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

        public Company FindByCondition(Expression<Func<Company, bool>> predicate)
        {
            Company company = _companyRepository.FindByCondition(predicate);

            return company;
        }

        public CurrentResponse UpdateImageName(int id, string logoName)
        {
            try
            {
                bool isImageNameUpdated = _companyRepository.UpdateImageName(id, logoName);

                Company company = _companyRepository.FindByCondition(p => p.Id == id && p.IsDeleted != true && p.IsActive == true);
                company.Logo = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.CompanyLogo}/{company.Logo}";

                CreateResponse(company, HttpStatusCode.OK, "Company logo updated successfully");

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

        public CurrentResponse IsCompanyExist(int id, string name)
        {
            try
            {
                CompanyVM companyVM = new CompanyVM() { Id = id, Name = name };

                bool isCompanyExist = IsCompanyExist(companyVM);

                if (isCompanyExist)
                {
                    CreateResponse(isCompanyExist, HttpStatusCode.OK, "Company name is already exist.");
                }
                else
                {
                    CreateResponse(isCompanyExist, HttpStatusCode.OK, "");
                }

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse IsDisplayPropeller(int id)
        {
            try
            {
                bool isDisplayPropeller = _companyRepository.FindByCondition(p=>p.Id == id).IsDisplayPropeller;

                CreateResponse(isDisplayPropeller, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse UpdateCreatedBy(int id, long createdBy)
        {
            try
            {
                Company company = _companyRepository.FindByCondition(p => p.Id == id);

                if (company == null)
                {
                    CreateResponse(company, HttpStatusCode.NotFound, "Company not found");
                    return _currentResponse;
                }

                company.CreatedBy = createdBy;
                company = _companyRepository.Edit(company);

                CreateResponse(company, HttpStatusCode.OK, "Company details updated successfully");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        #region Object Mapper

        private CompanyVM ToBusinessObject(Company company)
        {
            CompanyVM companyVM = new CompanyVM();

            companyVM.Id = company.Id;
            companyVM.Name = company.Name;
            companyVM.Address = company.Address;
            companyVM.City = company.City;
            companyVM.State = company.State;
            companyVM.Zipcode = company.Zipcode;
            companyVM.ContactNo = company.ContactNo;
            companyVM.TimeZone = company.TimeZone;
            companyVM.Website = company.Website;
            companyVM.PrimaryAirport = company.PrimaryAirport;
            companyVM.PrimaryServiceId = company.PrimaryServiceId;
            companyVM.Logo = company.Logo;
            companyVM.IsDisplayPropeller = company.IsDisplayPropeller;

            companyVM.LogoPath = $"{Configuration.ConfigurationSettings.Instance.UploadDirectoryPath}/{UploadDirectories.CompanyLogo}/{companyVM.Logo}";

            return companyVM;
        }

        public Company ToDataObject(CompanyVM companyVM)
        {
            Company company = new Company();

            company.Id = companyVM.Id;
            company.Name = companyVM.Name;
            company.Address = companyVM.Address;
            company.City = companyVM.City;
            company.State = companyVM.State;
            company.Zipcode = companyVM.Zipcode;
            company.ContactNo = companyVM.ContactNo;
            company.TimeZone = companyVM.TimeZone;
            company.Website = companyVM.Website;
            company.PrimaryAirport = companyVM.PrimaryAirport;
            company.PrimaryServiceId = companyVM.PrimaryServiceId == null ? null : (short)companyVM.PrimaryServiceId;
            company.IsDisplayPropeller = companyVM.IsDisplayPropeller;

            company.IsActive = true;
            company.CreatedBy = companyVM.CreatedBy;

            if (companyVM.Id == 0)
            {
                company.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                company.UpdatedBy = companyVM.UpdatedBy;
                company.UpdatedOn = DateTime.UtcNow;
            }

            return company;
        }

        #endregion
    }
}
