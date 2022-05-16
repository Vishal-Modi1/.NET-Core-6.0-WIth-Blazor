using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using System.Collections.ObjectModel;
using FSM.Blazor.Utilities;

using Microsoft.AspNetCore.Components.Authorization;
using DataModels.Constants;
using Radzen.Blazor;

namespace FSM.Blazor.Pages.Company
{
    public partial class Create
    {

        [Parameter] public CompanyVM companyData { get; set; }

        [Parameter] public Action GoToNext { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isLoading = false, isBusy = false,isAuthenticated = false;

        ReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
        int? primaryServiceId;

        public RadzenTemplateForm<CompanyVM> compnayForm;

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            isAuthenticated = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result);

            primaryServiceId = companyData.PrimaryServiceId;

            return base.OnInitializedAsync();
        }

        public async Task Submit(CompanyVM companyData)
        {
            isLoading = true;
            SetSaveButtonState(true);

            companyData.PrimaryServiceId = primaryServiceId == null ? null : (short)primaryServiceId;
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.SaveandUpdateAsync(dependecyParams, companyData);

            SetSaveButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Company Details", response.Message);
                NotificationService.Notify(message);

                CloseDilaog(false);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Company Details", response.Message);
                NotificationService.Notify(message);
            }

            isLoading = false;
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }

        public void CloseDilaog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        async void GotoNextStep()
        {
            if (!compnayForm.EditContext.Validate())
            {
                return;
            }

            GoToNext.Invoke();
        }

        void RedirectToLogin()
        {
            NavigationManager.NavigateTo("/Login");
        }
    }
}
