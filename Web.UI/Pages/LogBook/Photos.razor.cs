using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using DataModels.VM.LogBook;
using DataModels.Enums;
using DataModels.VM.Common;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook
{
    partial class Photos
    {
        [Parameter] public EventCallback OnToggle { get; set; }
        [Parameter] public bool ShowBodyPart { get; set; }
        [Parameter] public List<LogBookFlightPhotoVM> PhotosList { get; set; }

        LogBookFlightPhotoVM logBookFlightPhotoVM  { get; set; }

        protected override Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            return base.OnInitializedAsync();
        }

        async Task<bool> TriggerToggle()
        {
            await OnToggle.InvokeAsync();
            return true;
        }

        async Task OnFileChanged(InputFileChangeEventArgs e)
        {
            try
            {
                int i = 0;
                foreach (var item in e.GetMultipleFiles())
                {
                    i++;
                    var image = await item.RequestImageFileAsync("image/png", 600, 600);
                    using Stream imageStream = image.OpenReadStream(1024 * 1024 * 10);

                    using MemoryStream ms = new();
                    await imageStream.CopyToAsync(ms);

                    LogBookFlightPhotoVM logBookFlightPhotoVM = new LogBookFlightPhotoVM();
                    logBookFlightPhotoVM.DisplayName = item.Name;
                    logBookFlightPhotoVM.ImagePath = $"data:image/png;base64,{Convert.ToBase64String(ms.ToArray())}";
                    logBookFlightPhotoVM.ImageData = ms.ToArray();
                    logBookFlightPhotoVM.Name = "";

                    PhotosList.Add(logBookFlightPhotoVM);
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        async Task OpenDeleteDialog(LogBookFlightPhotoVM selectedPhoto)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Photo";

            logBookFlightPhotoVM = selectedPhoto;
        }

        async Task DeleteAsync()
        {
            isBusyDeleteButton = true;

            CurrentResponse response = await LogBookService.DeletePhotoAsync(dependecyParams, logBookFlightPhotoVM.Id);

            isBusyDeleteButton = false;

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                isDisplayPopup = false;

                PhotosList.Remove(logBookFlightPhotoVM);
            }

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
        }
    }
}
