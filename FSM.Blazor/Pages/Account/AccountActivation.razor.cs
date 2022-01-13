using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Radzen;

namespace FSM.Blazor.Pages.Account
{
    partial class AccountActivation
    {
        string message;
        public string Link { get; set; }

        public bool IsValidToken { get; set; }

        public bool ShowError { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        protected override async void OnInitialized()
        {
            base.OnInitialized();
            
            StringValues link;

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out link);


            if (link.Count() == 0 || link[0] == "")
            {
                message = "Token is not exist. Please try with valid token!";
                ShowError = true;
                IsValidToken = false;
            }
            else
            {
                Link = link[0];
                message = "Validating token ...";
                StateHasChanged();

                CurrentResponse response = await AccountService.ValidateResetPasswordTokenAsync(_httpClient, Link);
                ManageResponseAsync(response);
                ShowError = false;
            }
        }

        private async Task ManageResponseAsync(CurrentResponse response)
        {
            if (response == null)
            {
                message = "Token is not valid. Please try with valid token!";
                IsValidToken = false;
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    response = await AccountService.ActivateAccountAsync(_httpClient, Link);
                    IsValidToken = true;

                    message = response.Message;
                    StateHasChanged();
                }
                else
                {
                    message = "Token is not exist! Please try with valid token!";
                    IsValidToken = false;
                    StateHasChanged();
                }
            }
            else
            {
                message = response.Message;
                IsValidToken = false;
                StateHasChanged();
            }
        }

        public async Task OnLoginButtonClick()
        {
            NavigationManager.NavigateTo("/Login");
        }
    }
}
