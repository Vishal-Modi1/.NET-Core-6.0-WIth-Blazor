using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Company;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Security.Claims;
using Web.UI.Utilities;

namespace Web.UI.Pages.Account
{
    partial class Registration
    {
        public CompanyVM companyData = new CompanyVM();
        public UserVM userVM = new UserVM();

        int currentStep = 0;
        bool isLoading, isValidToken, showError;
        string link;

        protected override Task OnInitializedAsync()
        {
            ChangeLoaderVisibilityAction(true);
            return base.OnInitializedAsync();
        }

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

                        //globalMembers.UINotification.DisplayInfoNotification(globalMembers.UINotification.Instance, "Validating token ...");

                        dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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

                ChangeLoaderVisibilityAction(false);
                base.StateHasChanged();
            }
        }

        private async Task ManageResponseAsync(CurrentResponse response)
        {
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                userVM = await UserService.GetMasterDetailsAsync(dependecyParams, true, link);
                await GetCompanyDetails(dependecyParams);

                isValidToken = true;
               // globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, response.Message);
            }
            else
            {
                NavigationManager.NavigateTo("/TokenExpired");
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
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
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
            ChangeLoaderVisibilityAction(true);
            base.StateHasChanged();

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

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

                if (response == null)
                {
                    globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
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
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
                }
            }
            else
            {
                userVM.ActivationLink = NavigationManager.BaseUri + "AccountActivation?Token=";
                CurrentResponse response = await UserService.SaveandUpdateAsync(dependecyParams, userVM);

                await ManageUserCreateResponseAsync(response);
            }

            ChangeLoaderVisibilityAction(false);
            base.StateHasChanged();
        }

        private async Task ManageUserCreateResponseAsync(CurrentResponse response)
        {
            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                userVM = JsonConvert.DeserializeObject<UserVM>(response.Data.ToString());
                await UpdateCreatedByAsync(userVM);
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }
        }

        private async Task UpdateCreatedByAsync(UserVM userVM)
        {
            companyData.CreatedBy = userVM.Id;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            var response = await CompanyService.UpdateCreatedByAsync(dependecyParams, companyData.Id, userVM.Id);

            if (response == null)
            {
                globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                NavigationManager.NavigateTo("/RegistrationSuccess");
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
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
            bool isEmailExist = false;

            if (response == null)
            {
                globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);

            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isEmailExist = true;
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Email is already exist!");
                }
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
            }

            return isEmailExist;
        }

        private bool ManageIsCompanyExistResponse(CurrentResponse response, string summary)
        {
            bool isCompanyExist = false;

            if (response == null)
            {
                globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isCompanyExist = true;
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "Company is already exist!");
                }
            }
            else
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
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
