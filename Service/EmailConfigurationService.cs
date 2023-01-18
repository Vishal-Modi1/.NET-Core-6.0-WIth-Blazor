using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class EmailConfigurationService : BaseService, IEmailConfigurationService
    {
        private readonly IEmailConfigurationRepository _emailConfigurationRepository;

        public EmailConfigurationService(IEmailConfigurationRepository emailConfigurationRepository)
        {
            _emailConfigurationRepository = emailConfigurationRepository;
        }

        public CurrentResponse Create(EmailConfigurationVM emailConfigurationVM)
        {
            try
            {
                EmailConfiguration emailConfiguration = new EmailConfiguration();

                emailConfiguration.Email = emailConfigurationVM.Email;
                emailConfiguration.CompanyId = emailConfigurationVM.CompanyId;
                emailConfiguration.EmailTypeId = (byte)emailConfigurationVM.EmailTypeId;

                _emailConfigurationRepository.Create(emailConfiguration);

                CreateResponse(emailConfiguration, HttpStatusCode.OK, "Email configured successfully");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(EmailConfigurationVM emailConfigurationVM)
        {
            try
            {
                EmailConfiguration emailConfiguration = new EmailConfiguration();

                emailConfiguration.Id = emailConfigurationVM.Id;
                emailConfiguration.Email = emailConfigurationVM.Email;
                emailConfiguration.CompanyId = emailConfigurationVM.CompanyId;
                emailConfiguration.EmailTypeId = (byte)emailConfigurationVM.EmailTypeId;

                _emailConfigurationRepository.Edit(emailConfiguration);
                CreateResponse(emailConfiguration, HttpStatusCode.OK, "Email configured successfully");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(long id)
        {
            try
            {
                EmailConfiguration emailConfiguration = _emailConfigurationRepository.FindByCondition(p => p.Id == id);
                EmailConfigurationVM emailConfigurationVM = new EmailConfigurationVM();

                if (emailConfiguration != null)
                {
                    emailConfigurationVM.Id = emailConfiguration.Id;
                    emailConfigurationVM.Email = emailConfiguration.Email;
                    emailConfigurationVM.CompanyId = emailConfiguration.CompanyId;
                    emailConfigurationVM.EmailTypeId = emailConfiguration.EmailTypeId;
                }

                emailConfigurationVM.EmailTypesList = _emailConfigurationRepository.ListDropdownEmailTypes();

                CreateResponse(emailConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetailsByEmailTypeAndCompanyId(int emailTypeId, int companyId)
        {
            try
            {
                EmailConfiguration emailConfiguration = _emailConfigurationRepository.FindByCondition(p => p.EmailTypeId == (byte)emailTypeId && p.CompanyId == companyId);
                EmailConfigurationVM emailConfigurationVM = new EmailConfigurationVM();

                if (emailConfiguration != null)
                {
                    emailConfigurationVM.Id = emailConfiguration.Id;
                    emailConfigurationVM.Email = emailConfiguration.Email;
                    emailConfigurationVM.CompanyId = emailConfiguration.CompanyId;
                    emailConfigurationVM.EmailTypeId = emailConfiguration.EmailTypeId;
                }
                else
                {
                    emailConfigurationVM.CompanyId = companyId;
                    emailConfigurationVM.EmailTypeId = emailTypeId;
                }

                CreateResponse(emailConfigurationVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception ex)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, ex.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List(DatatableParams datatableParams)
        {
            try
            {
                List<EmailConfigurationDataVM> disperanciesList = _emailConfigurationRepository.List(datatableParams);
                CreateResponse(disperanciesList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
