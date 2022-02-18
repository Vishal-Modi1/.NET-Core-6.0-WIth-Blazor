using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.UserPreference;
using Repository.Interface;
using Service.Interface;
using System;
using System.Net;

namespace Service
{
    public class UserPreferenceService : BaseService, IUserPreferenceService
    {
        private readonly IUserPreferenceRepository _userPreferenceRepository;

        public UserPreferenceService(IUserPreferenceRepository userPreferenceRepository)
        {
            _userPreferenceRepository = userPreferenceRepository;
        }

        public CurrentResponse Create(UserPreferenceVM userPreferenceVM)
        {
            UserPreference userPreference = ToDataObject(userPreferenceVM);

            try
            {
                userPreference = _userPreferenceRepository.Create(userPreference);
                CreateResponse(userPreference, HttpStatusCode.OK, "Preference added successfully");

                return _currentResponse;
            }
            catch (Exception exc)
            {
                CreateResponse(null, HttpStatusCode.InternalServerError, exc.ToString());

                return _currentResponse;
            }
        }

        private UserPreference ToDataObject(UserPreferenceVM userPreferenceVM)
        {
            UserPreference userPreference = new UserPreference();

            userPreference.Id = userPreferenceVM.Id;
            userPreference.PreferenceType = userPreferenceVM.PreferenceType.ToString();
            userPreference.UserId = userPreferenceVM.UserId;
            userPreference.PreferencesIds = String.Join(',', userPreferenceVM.ListPreferencesIds);

            return userPreference;
        }
    }
}
