using DataModels.VM.Common;
using DataModels.VM.Company;
using DataModels.VM.User;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;

namespace FSM.Blazor.Pages.Account
{
    partial class Registration
    {
        #region Params

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        #endregion

        public CompanyVM companyData = new CompanyVM();
        public UserVM userVM = new UserVM();

        int currentStep = 0;
        bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

            userVM = await UserService.GetMasterDetailsAsync(dependecyParams);
        }

        void GoToNextStep()
        {
            currentStep++;

            userVM.CompanyName = companyData.Name;

            base.StateHasChanged();
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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
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

                ManageUserCreateResponse(response);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Company Details", response.Message);
                NotificationService.Notify(message);
            }

            isLoading = false;
            base.StateHasChanged();

            //RefreshPage();
        }

        private void ManageUserCreateResponse(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later or contact to our administrator department.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                //message = new NotificationMessage().Build(NotificationSeverity.Success, "Account registered successfully.", "We have sent verification link in your mail, Please confirm your account by click on verification link", 10000);
                //NotificationService.Notify(message);

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
    }
}
