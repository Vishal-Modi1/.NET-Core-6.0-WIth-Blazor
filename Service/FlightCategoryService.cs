using DataModels.Entities;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using DataModels.VM.Common;
using Service.Interface;
using DataModels.VM.Scheduler;
using AutoMapper;

namespace Service
{
    public class FlightCategoryService : BaseService, IFlightCategoryService
    {
        private readonly IFlightCategoryRepository _flightCategoryRepository;
        private readonly IMapper _mapper;

        public FlightCategoryService(IFlightCategoryRepository flightCategoryRepository, IMapper mapper)
        {
            _flightCategoryRepository = flightCategoryRepository;
            _mapper = mapper;
        }

        public CurrentResponse Create(FlightCategoryVM flightCategoryVM)
        {
            try
            {
                FlightCategory existingCategory = _flightCategoryRepository.FindByCondition(p => p.Id != flightCategoryVM.Id && p.Name == flightCategoryVM.Name && p.CompanyId == flightCategoryVM.CompanyId);

                if (existingCategory is not null)
                {
                    CreateResponse(existingCategory, HttpStatusCode.Ambiguous, "Category is already exist");
                }
                else
                {
                    FlightCategory flightCategory = _mapper.Map<FlightCategory>(flightCategoryVM);

                    if (flightCategoryVM.IsForAllCompanies)
                    {
                        _flightCategoryRepository.CreateForAllCompanies(flightCategory);
                    }
                    else
                    {
                        flightCategory = _flightCategoryRepository.Create(flightCategory);
                    }

                    CreateResponse(flightCategory, HttpStatusCode.OK, "Category added successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(FlightCategoryVM flightCategoryVM)
        {
            try
            {
                FlightCategory existingCategory = _flightCategoryRepository.FindByCondition(p=>p.Id != flightCategoryVM.Id && p.Name == flightCategoryVM.Name && p.CompanyId == flightCategoryVM.CompanyId);

                if (existingCategory is not null)
                {
                    CreateResponse(existingCategory, HttpStatusCode.Ambiguous, "Category is already exist");
                }
                else
                {
                    FlightCategory flightCategory = _mapper.Map<FlightCategory>(flightCategoryVM);
                    flightCategory = _flightCategoryRepository.Edit(flightCategory);
                    CreateResponse(flightCategory, HttpStatusCode.OK, "Category updated successfully");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListDropDownValues(int companyId)
        {
            try
            {
                List<DropDownValues> categoriesList = _flightCategoryRepository.ListDropDownValuesByCompanyId(companyId);

                CreateResponse(categoriesList, HttpStatusCode.OK, "");

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
                // pending to check already in use or not
                _flightCategoryRepository.Delete(id);
                CreateResponse(true, HttpStatusCode.OK, "Category deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse ListByCompanyId(int companyId)
        {
            try
            {
                List<FlightCategory> flightCategories =  _flightCategoryRepository.ListByCompanyId(companyId);
                List<FlightCategoryVM> flightCategoryVMs = _mapper.Map<List<FlightCategoryVM>>(flightCategories);

                CreateResponse(flightCategoryVMs, HttpStatusCode.OK, "Category deleted successfully.");

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
