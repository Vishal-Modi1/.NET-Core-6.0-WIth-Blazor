using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.Company.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.Company.Settings
{
    public class CompanyDateFormatService
    {
        private readonly HttpCaller _httpCaller;

        public CompanyDateFormatService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<CurrentResponse> SetDefault(DependecyParams dependecyParams, CompanyDateFormatVM companyDateFormatVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(companyDateFormatVM);
            dependecyParams.URL = $"companyDateFormat/setDefault";
            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<CompanyDateFormatVM> GetDefault(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "companyDateFormat/GetDefault";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                CompanyDateFormatVM CompanyDateFormatVM = JsonConvert.DeserializeObject<CompanyDateFormatVM>(response.Data.ToString());

                return CompanyDateFormatVM;
            }
            catch (Exception exc)
            {
                return new();
            }
        }

        public async Task<List<DropDownSmallValues>> ListDropDownValues(DependecyParams dependecyParams)
        {
            try
            {
                dependecyParams.URL = "companyDateFormat/listDropDownValues";
                CurrentResponse response = await _httpCaller.GetAsync(dependecyParams);

                List<DropDownSmallValues> dateFormats = JsonConvert.DeserializeObject<List<DropDownSmallValues>>(response.Data.ToString());

                return dateFormats;
            }
            catch (Exception exc)
            {
                return new();
            }
        }
    }
}
