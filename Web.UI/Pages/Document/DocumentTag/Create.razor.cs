using DataModels.VM.Common;
using Web.UI.Extensions;
using DataModels.VM.Document;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Document.DocumentTag
{
    public partial class Create
    {
        DocumentTagVM documentTagVM = new DocumentTagVM();
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentService.SaveTagAsync(dependecyParams, documentTagVM);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        public void CloseDialog(bool reloadList)
        {
            CloseDialogCallBack.InvokeAsync(reloadList);
        }
    }
}
