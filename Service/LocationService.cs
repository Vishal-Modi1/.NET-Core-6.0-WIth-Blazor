using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Location;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;

namespace Service
{
    public class LocationService : BaseService, ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public CurrentResponse Create(LocationVM locationVM)
        {
            Location location = ToDataObject(locationVM);

            try
            {
                bool isLocationExist = IsLocationExist(locationVM);

                if (isLocationExist)
                {
                    CreateResponse(locationVM, HttpStatusCode.Ambiguous, "Location with selected timezone is already exist");
                }
                else
                {
                    location = _locationRepository.Create(location);

                    locationVM = ToBusinessObject(location);

                    CreateResponse(locationVM, HttpStatusCode.OK, "Location added successfully");
                }

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
                _locationRepository.Delete(id, deletedBy);
                CreateResponse(true, HttpStatusCode.OK, "Location deleted successfully.");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(false, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse Edit(LocationVM locationVM)
        {
            Location location = ToDataObject(locationVM);

            try
            {
                bool isLocationExist = IsLocationExist(locationVM);

                if (isLocationExist)
                {
                    CreateResponse(locationVM, HttpStatusCode.Ambiguous, "Location with selected timezone is already exist");
                }
                else
                {
                    location = _locationRepository.Edit(location);

                    locationVM = ToBusinessObject(location);

                    CreateResponse(locationVM, HttpStatusCode.OK, "Location updated successfully");
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
                List<DropDownValues> locationsList = _locationRepository.ListDropDownValues(companyId);

                CreateResponse(locationsList, HttpStatusCode.OK, "");

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
                List<LocationDataVM> locationList = _locationRepository.List(datatableParams);

                CreateResponse(locationList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse GetDetails(int id)
        {
            Location location = _locationRepository.FindByCondition(p => p.Id == id);
            LocationVM locationVM = new LocationVM();

            if (location != null)
            {
                locationVM = ToBusinessObject(location);
            }

            CreateResponse(locationVM, HttpStatusCode.OK, "");

            return _currentResponse;
        }

        private bool IsLocationExist(LocationVM locationVM)
        {
            Location location = _locationRepository.FindByCondition(p=> p.Id != locationVM.Id && p.TimezoneId == locationVM.TimezoneId 
            && p.AirportCode == locationVM.AirportCode && p.CompanyId == locationVM.CompanyId);
        
            if(location != null)
            {
                return true;
            }

            return false;
        }

        #region Object Mapper

        private LocationVM ToBusinessObject(Location location)
        {
            LocationVM locationVM = new LocationVM();

            locationVM.Id = location.Id;
            locationVM.TimezoneId = location.TimezoneId;
            locationVM.AirportCode = location.AirportCode;
            locationVM.PhysicalAddress = location.PhysicalAddress;
            location.CompanyId = locationVM.CompanyId;

            return locationVM;
        }

        public Location ToDataObject(LocationVM locationVM)
        {
            Location location = new Location();

            location.Id = locationVM.Id;
            location.TimezoneId = locationVM.TimezoneId;
            location.AirportCode = locationVM.AirportCode;
            location.CompanyId = locationVM.CompanyId;
            location.PhysicalAddress = locationVM.PhysicalAddress;

            location.CreatedBy = locationVM.CreatedBy;
            location.UpdatedBy = locationVM.UpdatedBy;

            if (locationVM.Id == 0)
            {
                location.CreatedOn = DateTime.UtcNow;
            }

            location.UpdatedOn = DateTime.UtcNow;

            return location;
        }

        #endregion
    }
}
