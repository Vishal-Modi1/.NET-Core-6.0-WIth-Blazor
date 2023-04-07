using DataModels.VM.Common;
using Web.UI.Extensions;
using DataModels.VM.Document;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.Blazor;

namespace Web.UI.Pages.Document.DocumentTag
{
    public partial class Create
    {
        [Parameter] public DocumentTagVM documentTagVM { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentTagService.SaveandUpdateAsync(dependecyParams, documentTagVM);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }

            isBusySubmitButton = false;
        }

        public void CloseDialog(bool reloadList)
        {
            CloseDialogCallBack.InvokeAsync(reloadList);
        }
    }
}
