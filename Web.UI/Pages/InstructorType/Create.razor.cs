using DataModels.VM.Common;
using DataModels.VM.InstructorType;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.InstructorType
{
    public partial class Create
    {
        [Parameter] public InstructorTypeVM instructorTypeData { get; set; }
        
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await InstructorTypeService.SaveandUpdateAsync(dependecyParams, instructorTypeData);

            isBusySubmitButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
