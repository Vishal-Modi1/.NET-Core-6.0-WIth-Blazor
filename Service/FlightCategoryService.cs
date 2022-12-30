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
    public class FlightCategoryService : BaseService, IFlightCategoryService
    {
        private readonly IFlightCategoryRepository _flightCategoryRepository;

        public FlightCategoryService(IFlightCategoryRepository flightCategoryRepository)
        {
            _flightCategoryRepository = flightCategoryRepository;
        }

        public CurrentResponse Create(FlightCategory flightCategory)
        {
            try
            {
                FlightCategory existingCategory = _flightCategoryRepository.FindByCondition(p => p.Id != flightCategory.Id && p.Name == flightCategory.Name && p.CompanyId == flightCategory.CompanyId);

                if (existingCategory is not null)
                {
                    CreateResponse(existingCategory, HttpStatusCode.Ambiguous, "Category is already exist");
                }
                else
                {
                    flightCategory = _flightCategoryRepository.Create(flightCategory);
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

        public CurrentResponse Edit(FlightCategory flightCategory)
        {
            try
            {
                FlightCategory existingCategory = _flightCategoryRepository.FindByCondition(p=>p.Id != flightCategory.Id && p.Name == flightCategory.Name && p.CompanyId == flightCategory.CompanyId);

                if (existingCategory is not null)
                {
                    CreateResponse(existingCategory, HttpStatusCode.Ambiguous, "Category is already exist");
                }
                else
                {
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
                CreateResponse(flightCategories, HttpStatusCode.OK, "Category deleted successfully.");

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
