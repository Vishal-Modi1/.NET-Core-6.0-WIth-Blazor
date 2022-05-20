using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Company;
using DataModels.VM.User;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using System.Security.Claims;

namespace FSM.Blazor.Pages.Account
{
    partial class Registration
    {
        public CompanyVM companyData = new CompanyVM();
        public UserVM userVM = new UserVM();

        int currentStep = 0;
        bool isLoading;
        DependecyParams dependecyParams;

        protected override async Task OnInitializedAsync()
        {
            ClaimsPrincipal cp = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;

            string claimValue = cp.Claims.Where(c => c.Type == CustomClaimTypes.AccessToken)
                               .Select(c => c.Value).SingleOrDefault();

            if(!string.IsNullOrEmpty(claimValue))
            {
                NavigationManager.NavigateTo("/Login");
            }

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            userVM = await UserService.GetMasterDetailsAsync(dependecyParams);
        }

        async Task GoToNextStepAsync()
        {
            isLoading = true;

            bool isCompanyExist = await IsCompanyExistAsync();

            if (!isCompanyExist)
            {
                currentStep++;
                userVM.CompanyName = companyData.Name;
            }

            isLoading = false;
            base.StateHasChanged();
        }

        async Task<bool> IsCompanyExistAsync()
        {
            CurrentResponse response = await CompanyService.IsCompanyExistAsync(dependecyParams, companyData.Id, companyData.Name);

            bool isCompanyExist = ManageIsCompanyExistResponse(response, companyData.Name);

            return isCompanyExist;
        }

        void GoToBackStep()
        {
            currentStep--;
            base.StateHasChanged();
        }

        void SaveAsync()
        {
            SaveData();
        }

        async Task SaveData()
        {
            isLoading = true;
            base.StateHasChanged();
 
            CurrentResponse response = await UserService.IsEmailExistAsync(dependecyParams, userVM.Email);

            bool isEmailExist = ManageIsEmailExistResponse(response, userVM.Email);

            if (isEmailExist)
            {
                isLoading = false;
                base.StateHasChanged();
                return;
            }

            response = await CompanyService.SaveandUpdateAsync(dependecyParams, companyData);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later or contact to our administrator department.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM.ActivationLink = NavigationManager.BaseUri + "AccountActivation?Token=";

                companyData = JsonConvert.DeserializeObject<CompanyVM>(response.Data.ToString());

                userVM.CompanyId = companyData.Id;
                response = await UserService.SaveandUpdateAsync(dependecyParams, userVM);

                await ManageUserCreateResponseAsync(response);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Company Details", response.Message);
                NotificationService.Notify(message);
            }

            isLoading = false;
            base.StateHasChanged();
        }

        private async Task ManageUserCreateResponseAsync(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later or contact to our administrator department.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
                await UpdateCreatedByAsync(userVM);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "User Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        private async Task UpdateCreatedByAsync(UserVM userVM)
        {
            companyData.CreatedBy = userVM.Id;

            var response = await CompanyService.UpdateCreatedByAsync(dependecyParams, companyData.Id , userVM.Id);
           
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later or contact to our administrator department.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/RegistrationSuccess");
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "User Details", response.Message);
                NotificationService.Notify(message);
            }

        }

        private void RefreshPage()
        {
            companyData = new CompanyVM();
            userVM = new UserVM();

            isLoading = false;

          //  NavigationManager.NavigateTo("");

            base.StateHasChanged();
        }

        private bool ManageIsEmailExistResponse(CurrentResponse response, string summary)
        {
            NotificationMessage message;
            bool isEmailExist = false;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isEmailExist = true;
                    message = new NotificationMessage().Build(NotificationSeverity.Error, summary, "Email is already exist!");
                    NotificationService.Notify(message);
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }

            return isEmailExist;
        }

        private bool ManageIsCompanyExistResponse(CurrentResponse response, string summary)
        {
            NotificationMessage message;
            bool isCompanyExist = false;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isCompanyExist = true;
                    message = new NotificationMessage().Build(NotificationSeverity.Error, summary, "Company is already exist!");
                    NotificationService.Notify(message);
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }

            return isCompanyExist;
        }
    }
}
