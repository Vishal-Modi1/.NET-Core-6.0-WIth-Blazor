using DataModels.VM.Common;
using DataModels.VM.EmailConfiguration;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Web.UI.Data.Company.Settings;
using Web.UI.Utilities;

namespace Web.UI.Pages.Company.DetailsView.Settings.Notifications
{
    partial class Create
    {
        [Parameter] public EmailConfigurationVM EmailConfigurationData { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public EventCallback<EmailConfigurationVM> UpdateTabUI { get; set; }

        public EmailConfigurationVM emailConfiguration { get; set; }
        protected override Task OnInitializedAsync()
        {
            emailConfiguration = new EmailConfigurationVM();
            return base.OnInitializedAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await EmailConfigurationService.SaveandUpdateAsync(dependecyParams, emailConfiguration);
            
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                emailConfiguration = JsonConvert.DeserializeObject<EmailConfigurationVM>(response.Data.ToString());
                CloseDialog(true);
            }

            isBusySubmitButton = false;
        }

        public async Task OnEmailTypeValueChanged(int value)
        {
            if(value == 0)
            {
                return;
            }

            ChangeLoaderVisibilityAction(true);

            emailConfiguration.EmailTypeId = value;
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            emailConfiguration = await EmailConfigurationService.GetDetailsByEmailTypeAndCompanyIdAsync(dependecyParams, value, EmailConfigurationData.CompanyId);

            ChangeLoaderVisibilityAction(false);
            base.StateHasChanged();
        }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
