using DataModels.VM.Common;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace FSM.Blazor.Pages.Account
{
    partial class AccountActivation
    {
        string message;
        public string Link { get; set; }

        public bool IsValidToken { get; set; }

        public bool ShowError { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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

                    DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                    CurrentResponse response = await AccountService.ValidateTokenAsync(dependecyParams, Link);
                    await ManageResponseAsync(response);
                    ShowError = false;
                }
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
                    DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                    response = await AccountService.ActivateAccountAsync(dependecyParams, Link);
                    IsValidToken = true;

                    message = response.Message;
                    StateHasChanged();
                }
                else
                {
                    message = response.Message;
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
