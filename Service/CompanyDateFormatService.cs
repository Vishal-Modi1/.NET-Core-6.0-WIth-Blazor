using DataModels.VM.Common;
using DataModels.VM.Company.Settings;
using Service.Interface;
using System;
using DataModels.Entities;
using AutoMapper;
using Repository.Interface;
using System.Net;
using System.Collections.Generic;

namespace Service
{
    public class CompanyDateFormatService : BaseService, ICompanyDateFormatService
    {
        private readonly ICompanyDateFormatRepository _companyDateFormatRepository;
        private readonly IMapper _mapper;

        public CompanyDateFormatService(ICompanyDateFormatRepository
            companyDateFormatRepository, IMapper mapper)
        {
            _companyDateFormatRepository = companyDateFormatRepository;
            _mapper = mapper;
        }

        public CurrentResponse FindByCompanyId(long companyId)
        {
            try
            {
                CompanyDateFormatVM companyDateFormatVM = new();
                CompanyDateFormat data = _companyDateFormatRepository.FindByCondition(p => p.CompanyId == companyId);

                if (data is not null)
                {
                    companyDateFormatVM = _mapper.Map<CompanyDateFormatVM>(data);
                }

                CreateResponse(companyDateFormatVM, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public string FindDateFormatByCompanyId(int companyId)
        {
            string data = _companyDateFormatRepository.FindDateFormatValue(companyId);

            if (string.IsNullOrWhiteSpace(data))
            {
                data = "MM-dd-yyyy";
            }

            return data;
        }

        public CurrentResponse ListDropDownValues()
        {
            try
            {
                List<DropDownSmallValues> dateFormats = _companyDateFormatRepository.ListDropDownValues();

                CreateResponse(dateFormats, HttpStatusCode.OK, "");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(CompanyDateFormatVM companyDateFormatVM)
        {
            try
            {
                CompanyDateFormat companyDateFormat = _mapper.Map<CompanyDateFormat>(companyDateFormatVM);
                _companyDateFormatRepository.SetDefault(companyDateFormat);

                companyDateFormatVM = _mapper.Map<CompanyDateFormatVM>(companyDateFormat);

                CreateResponse(companyDateFormatVM, HttpStatusCode.OK, "Default value updated");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }
    }
}
