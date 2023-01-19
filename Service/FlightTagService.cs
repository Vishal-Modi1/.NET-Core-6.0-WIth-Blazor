using AutoMapper;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Scheduler;
using DataModels.VM.Weather;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FlightTagService : BaseService, IFlightTagService
    {
        private readonly IFlightTagRepository _flightTagRepository;
        private readonly IMapper _mapper;
        public FlightTagService(IFlightTagRepository flightTagRepository, IMapper mapper)
        {
            _flightTagRepository = flightTagRepository;
            _mapper = mapper;
        }

        public CurrentResponse Create(FlightTagVM flightTagVM)
        {
            try
            {
                bool isFlightTagExist = IsTagExist(flightTagVM.TagName);

                if (isFlightTagExist)
                {
                    CreateResponse(null, HttpStatusCode.Ambiguous, "Flight tag is already exist");
                    return _currentResponse;
                }

                FlightTag flightTag = ToDataObject(flightTagVM);
                flightTag = _flightTagRepository.Create(flightTag);
                CreateResponse(flightTag, HttpStatusCode.OK, "Flight tag added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse List()
        {
            try
            {
                List<FlightTagVM> flightTagsList = _flightTagRepository.List();
                CreateResponse(flightTagsList, HttpStatusCode.OK, "");

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
                List<DropDownLargeValues> flightTagsList = _flightTagRepository.ListDropDownValues();
                CreateResponse(flightTagsList, HttpStatusCode.OK, "");

                return _currentResponse;
            }

            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public bool IsTagExist(string tag)
        {
            FlightTagVM flightTag = _flightTagRepository.FindByCondition(p => p.TagName.ToLower() == tag.ToLower() && p.IsActive == true && p.IsDeleted == false);

            if (flightTag == null)
            {
                return false;
            }

            return true;
        }

        private FlightTag ToDataObject(FlightTagVM flightTagVM)
        {
            FlightTag flightTag = new FlightTag();

            flightTag =  _mapper.Map<FlightTag>(flightTagVM);
            flightTag.CreatedBy = flightTagVM.CreatedBy;

            if (flightTagVM.Id == 0)
            {
                flightTag.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                flightTag.UpdatedBy = flightTagVM.UpdatedBy;

                flightTag.UpdatedOn = DateTime.UtcNow;
            }

            return flightTag;
        }
    }
}
