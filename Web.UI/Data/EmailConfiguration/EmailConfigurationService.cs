using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Web.UI.Utilities;

namespace Web.UI.Data.EmailConfiguration
{
    public class EmailConfigurationService
    {
        private readonly HttpCaller _httpCaller;

        public EmailConfigurationService(AuthenticationStateProvider authenticationStateProvider)
        {
            _httpCaller = new HttpCaller(authenticationStateProvider);
        }

        public async Task<List<EmailConfigurationDataVM>> ListAsync(DependecyParams dependecyParams, DatatableParams datatableParams)
        {
            try
            {
                dependecyParams.URL = "emailConfiguration/list";
                dependecyParams.JsonData = JsonConvert.SerializeObject(datatableParams);
                CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

                List<EmailConfigurationDataVM> discrepanciesList = JsonConvert.DeserializeObject<List<EmailConfigurationDataVM>>(response.Data.ToString());

                return discrepanciesList;
            }
            catch (Exception exc)
            {
                return new List<EmailConfigurationDataVM>();
            }
        }

        public async Task<CurrentResponse> SaveandUpdateAsync(DependecyParams dependecyParams, EmailConfigurationVM emailConfigurationVM)
        {
            dependecyParams.JsonData = JsonConvert.SerializeObject(emailConfigurationVM);
            dependecyParams.URL = "emailConfiguration/create";

            if (emailConfigurationVM.Id > 0)
            {
                dependecyParams.URL = "emailConfiguration/edit";
            }

            CurrentResponse response = await _httpCaller.PostAsync(dependecyParams);

            return response;
        }

        public async Task<EmailConfigurationVM> GetDetailsAsync(DependecyParams dependecyParams, long id)
        {
            dependecyParams.URL = $"emailConfiguration/getDetails?id={id}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            EmailConfigurationVM emailConfigurationVM = new EmailConfigurationVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                emailConfigurationVM = JsonConvert.DeserializeObject<EmailConfigurationVM>(response.Data.ToString());
            }

            return emailConfigurationVM;
        }

        public async Task<EmailConfigurationVM> GetDetailsByEmailTypeAndCompanyIdAsync(DependecyParams dependecyParams, int emailTypeId, int companyId)
        {
            dependecyParams.URL = $"emailConfiguration/getDetailsByEmailTypeAndCompanyId?emailTypeId={emailTypeId}&companyId={companyId}";
            var response = await _httpCaller.GetAsync(dependecyParams);

            EmailConfigurationVM emailConfigurationVM = new EmailConfigurationVM();

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                emailConfigurationVM = JsonConvert.DeserializeObject<EmailConfigurationVM>(response.Data.ToString());
            }

            return emailConfigurationVM;
        }
    }
}
