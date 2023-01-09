using DataModels.VM.Common;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Web.UI.Pages.Account
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
                ChangeLoaderVisibilityAction(true);
                StringValues link;

                var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
                QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out link);

                if (link.Count() == 0 || link[0] == "")
                {
                    globalMembers.UINotification.DisplayErrorNotification(globalMembers.UINotification.Instance);
                }
                else
                {
                    Link = link[0];
                    //message = "Validating token ...";
                    //StateHasChanged();

                    dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                    CurrentResponse response = await AccountService.ValidateTokenAsync(dependecyParams, Link);
                    await ManageResponseAsync(response);
                    ShowError = false;
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

                response = await AccountService.ActivateAccountAsync(dependecyParams, Link);
                IsValidToken = true;

                message = response.Message;
                StateHasChanged();
            }
            else
            {
                NavigationManager.NavigateTo("/TokenExpired");
            }
        }

        public async Task OnLoginButtonClick()
        {
            NavigationManager.NavigateTo("/Login");
        }
    }
}
