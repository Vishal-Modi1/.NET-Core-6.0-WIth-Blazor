using DataModels.VM.Common;
using DataModels.VM.Company;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using Web.UI.Utilities;
using DataModels.Constants;

namespace Web.UI.Pages.Company
{
    public partial class Create
    {
        [Parameter] public CompanyVM companyData { get; set; }
        [Parameter] public bool IsInvited { get; set; }

        [Parameter] public Action GoToNext { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        bool isAuthenticated = false;

        ReadOnlyCollection<TimeZoneInfo> timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
        int? primaryServiceId;

        //public TelerikForm<CompanyVM> compnayForm;

        protected override Task OnParametersSetAsync()
        {
            primaryServiceId = companyData.PrimaryServiceId;
            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            isAuthenticated = !string.IsNullOrWhiteSpace(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result);
            primaryServiceId = companyData.PrimaryServiceId;

            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            companyData.PrimaryServiceId = primaryServiceId == null ? null : (short)primaryServiceId;
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyService.SaveandUpdateAsync(dependecyParams, companyData);

            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                 CloseDialog(true);
            }
            else
            {
                 CloseDialog(false);
            }

            isBusySubmitButton = false;
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        async void GotoNextStep()
        {
            //TOD:
            //if (!compnayForm.EditContext.Validate())
            //{
            //    return;
            //}

            GoToNext.Invoke();
        }

        void RedirectToLogin()
        {
            NavigationManager.NavigateTo("/Login");
        }
    }
}
