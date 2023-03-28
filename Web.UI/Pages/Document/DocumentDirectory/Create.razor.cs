using DataModels.VM.Common;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Document.DocumentDirectory;

namespace Web.UI.Pages.Document.DocumentDirectory
{
    partial class Create
    {
        [Parameter] public DocumentDirectoryVM documentDirectory { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        private async Task Save()
        {
            isBusySubmitButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentDirectoryService.SaveandUpdateAsync(dependecyParams, documentDirectory);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }

            isBusySubmitButton = false;
        }

        public void CloseDialog(bool refreshList)
        {
            CloseDialogCallBack.InvokeAsync(refreshList);
        }
    }
}
