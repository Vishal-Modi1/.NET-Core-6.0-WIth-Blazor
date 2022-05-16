using DataModels.VM.Common;
using DataModels.VM.InstructorType;
using Microsoft.AspNetCore.Components;
using FSM.Blazor.Extensions;
using Radzen;
using FSM.Blazor.Utilities;

namespace FSM.Blazor.Pages.InstructorType
{
    public partial class Create
    {
        [Parameter]
        public InstructorTypeVM InstructorTypeData { get; set; }
        
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isBusy = false;

        public async Task Submit(InstructorTypeVM instructorTypeData)
        {
            SetSaveButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InstructorTypeService.SaveandUpdateAsync(dependecyParams, instructorTypeData);

            SetSaveButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (((int)response.Status) == 200)
            {
                CloseDilaog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Instructor Type Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Instructor Type Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        public void CloseDilaog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
