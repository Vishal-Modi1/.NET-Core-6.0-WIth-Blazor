using DataModels.Entities;
using DataModels.VM.Common;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class UserAirTrafficControlCenterService : BaseService, IUserAirTrafficControlCenterService
    {
        private readonly IUserAirTrafficControlCenterRepository _userAirTrafficControlCenterRepository;

        public UserAirTrafficControlCenterService(IUserAirTrafficControlCenterRepository userAirTrafficControlCenterRepository)
        {
            _userAirTrafficControlCenterRepository = userAirTrafficControlCenterRepository;
        }

        public CurrentResponse FindByUserId(long userId)
        {
            try
            {
                UserAirTrafficControlCenter data = _userAirTrafficControlCenterRepository.FindByCondition(p=>p.UserId == userId);

                if (data == null)
                {
                    CreateResponse(0, HttpStatusCode.OK, "");
                }
                else
                {
                    CreateResponse(data.AirTrafficControlCenterId, HttpStatusCode.OK, "");
                }

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(0, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        public CurrentResponse SetDefault(UserAirTrafficControlCenter userAirTrafficControlCenter)
        {
            try
            {
                _userAirTrafficControlCenterRepository.SetDefault(userAirTrafficControlCenter);
                CreateResponse(true, HttpStatusCode.OK, "Default value updated");

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
