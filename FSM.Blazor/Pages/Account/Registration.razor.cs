using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Company;
using DataModels.VM.User;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
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
        bool isLoading, isValidToken, showError;
        DependecyParams dependecyParams;

        string link;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                StringValues url;

                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out url);

                try
                {
                    if (url.Count() > 0 && url[0] != "")
                    {
                        this.link = url[0];
                        NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Info, "", "Validating token ...");
                        NotificationService.Notify(message);
                        StateHasChanged();

                        DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                        CurrentResponse response = await AccountService.ValidateTokenAsync(dependecyParams, this.link);
                        await ManageResponseAsync(response);
                        showError = false;
                    }

                    else
                    {
                        ClaimsPrincipal cp = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;

                        string claimValue = cp.Claims.Where(c => c.Type == CustomClaimTypes.AccessToken)
                                           .Select(c => c.Value).SingleOrDefault();

                        if (!string.IsNullOrEmpty(claimValue))
                        {
                            NavigationManager.NavigateTo("/Login");
                        }

                        dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                        companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);

                        userVM = await UserService.GetMasterDetailsAsync(dependecyParams, false, "");
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
           
        }

        private async Task ManageResponseAsync(CurrentResponse response)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", "Token is not valid. Please try with valid token!");
                NotificationService.Notify(message);
                isValidToken = false;
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                    userVM = await UserService.GetMasterDetailsAsync(dependecyParams, true, link);
                    await GetCompanyDetails(dependecyParams);

                    isValidToken = true;
                    message = new NotificationMessage().Build(NotificationSeverity.Success, "", response.Message);
                    NotificationService.Notify(message);

                    StateHasChanged();
                }
                else
                {
                    message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                    NotificationService.Notify(message);

                    isValidToken = false;
                    StateHasChanged();
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "", response.Message);
                NotificationService.Notify(message);

                isValidToken = false;
                StateHasChanged();
            }
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
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.IsCompanyExistAsync(dependecyParams, companyData.Id, companyData.Name);

            bool isCompanyExist = ManageIsCompanyExistResponse(response, companyData.Name);

            return isCompanyExist;
        }

        void GoToBackStep()
        {
            isLoading = true;
            currentStep--;

            isLoading = false;
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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

            if (!userVM.IsInvited)
            {
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
            }
            else
            {
                userVM.ActivationLink = NavigationManager.BaseUri + "AccountActivation?Token=";
                CurrentResponse response = await UserService.SaveandUpdateAsync(dependecyParams, userVM);

                await ManageUserCreateResponseAsync(response);
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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

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

        private async Task GetCompanyDetails(DependecyParams dependecyParams)
        {
            CurrentResponse response = await CompanyService.GetDetailsAsync(dependecyParams, userVM.CompanyId.GetValueOrDefault());
            companyData = JsonConvert.DeserializeObject<CompanyVM>(response.Data.ToString());
            companyData.PrimaryServicesList = await CompanyService.ListCompanyServiceDropDownValues(dependecyParams);
        }
    }
}
