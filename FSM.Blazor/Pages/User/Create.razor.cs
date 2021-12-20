using DataModels.VM.Common;
using DataModels.VM.User;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;


namespace FSM.Blazor.Pages.User
{
    public partial class Create
    {
        [Parameter]
        public UserDataVM userData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }


        public async Task Submit(UserDataVM userDataVM)
        {
            CurrentResponse response = await UserService.SaveandUpdateAsync(_httpClient, userDataVM);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                dialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "User Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "User Details", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
