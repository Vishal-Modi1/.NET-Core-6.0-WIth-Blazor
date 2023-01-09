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
using DataModels.VM.Scheduler;

namespace Service
{
    public class FlightCategoryService : BaseService, IFlightCategoryService
    {
        private readonly IFlightCategoryRepository _flightCategoryRepository;

        public FlightCategoryService(IFlightCategoryRepository flightCategoryRepository)
        {
            _flightCategoryRepository = flightCategoryRepository;
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
                    FlightCategory flightCategory = ToDataObject(flightCategoryVM);
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
                    FlightCategory flightCategory = ToDataObject(flightCategoryVM);
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
                List<FlightCategoryVM> flightCategoryVMs  = ToBusinessObjectList(flightCategories);
                
                CreateResponse(flightCategoryVMs, HttpStatusCode.OK, "Category deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private List<FlightCategoryVM> ToBusinessObjectList(List<FlightCategory> flightCategories)
        {
            List<FlightCategoryVM> flightCategoryVMs = new List<FlightCategoryVM>();

            foreach (FlightCategory flightCategory in flightCategories)
            {
                FlightCategoryVM flightCategoryVM = ToBusinessObject(flightCategory);
                flightCategoryVMs.Add(flightCategoryVM);
            }

            return flightCategoryVMs;
        }

        private FlightCategoryVM ToBusinessObject(FlightCategory flightCategory)
        {
            FlightCategoryVM flightCategoryVM = new FlightCategoryVM();

            flightCategoryVM.Id = flightCategory.Id;
            flightCategoryVM.Name = flightCategory.Name;
            flightCategoryVM.CompanyId = flightCategory.CompanyId;
            flightCategoryVM.Color = flightCategory.Color;
            
            return flightCategoryVM;
        }

        private FlightCategory ToDataObject(FlightCategoryVM flightCategoryVM)
        {
            FlightCategory flightCategory = new FlightCategory();

            flightCategory.Id = flightCategoryVM.Id;
            flightCategory.Name = flightCategoryVM.Name;
            flightCategory.CompanyId = flightCategoryVM.CompanyId;
            flightCategory.Color = flightCategoryVM.Color;

            return flightCategory;
        }
    }
}
