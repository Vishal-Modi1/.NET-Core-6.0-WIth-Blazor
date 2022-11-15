using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Web.UI.Shared.Components.FileUploader
{
    partial class CustomFileUpload
    {
        [Parameter] public string DisplayName { get; set; }
        [Parameter] public bool IsFileUploadHasError { get; set; }
        [Parameter] public EventCallback<InputFileChangeEventArgs> OnInputFileChangeCallback { get; set; }
        [Parameter] public bool IsFileAdded {get;set;}

        async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            await OnInputFileChangeCallback.InvokeAsync(e);
        }
    }
}
