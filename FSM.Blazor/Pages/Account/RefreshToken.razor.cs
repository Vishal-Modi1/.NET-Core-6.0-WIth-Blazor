using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using FSM.Blazor.Utilities;
using DataModels.Constants;
using Radzen;
using FSM.Blazor.Extensions;
using DataModels.VM.Common;
using Newtonsoft.Json;

namespace FSM.Blazor.Pages.Account
{
    public partial class RefreshToken
    {
        [Parameter]
        public string userData { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        private string? result;
        private DotNetObjectReference<RefreshToken>? objRef;
        bool isTokenValid = true;

        async void OpenDocumentPreviewPopupAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            string refreshToken = _currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.RefreshToken).Result;
            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);

            var authModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await authModule.InvokeVoidAsync("RefreshToken", refreshToken, userId);

            objRef = DotNetObjectReference.Create(this);
            result = await authModule.InvokeAsync<string>("SetDotNetObject", objRef, "");
        }

        [JSInvokable]
        public void ManageRefreshTokenResponse(object response)
        {
            CurrentResponse responseObject = JsonConvert.DeserializeObject<CurrentResponse>(response.ToString());

            if(responseObject.Status == System.Net.HttpStatusCode.OK)
            {
                NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Success, responseObject.Message, "");
                NotificationService.Notify(message);

                NavigationManager.NavigateTo(NavigationManager.Uri);
            }
            else
            {
                isTokenValid = false;
                NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Error, responseObject.Message, "");
                NotificationService.Notify(message);
            }
        }
    }
}
