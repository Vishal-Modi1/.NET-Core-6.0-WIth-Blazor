using Configuration;
using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Discrepancy;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Web.UI.Utilities;

namespace Web.UI.Pages.Aircraft.DetailsTabs.Discrepancy.DiscrepancyFile
{
    partial class Create
    {
        [Parameter] public DiscrepancyFileVM discrepancyFileVM { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        bool isFileUploadHasError, isFileAdded;
        string errorMessage;
        IReadOnlyList<IBrowserFile> selectedFiles;
        public string SaveUrl => ToAbsoluteUrl("api/fileupload/save");

        public long maxFileSize = ConfigurationSettings.Instance.MaxDocumentUploadSize;
        List<string> supportedDocumentsFormat = ConfigurationSettings.Instance.SupportedImageTypes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        int maxSizeInMB = 10;
        string uploadedFilePath;

        public string ToAbsoluteUrl(string url)
        {
            return $"{NavigationManager.BaseUri}{url}";
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            byte[] fileData = new byte[0];

            if (!string.IsNullOrWhiteSpace(uploadedFilePath))
            {
                fileData = File.ReadAllBytes(uploadedFilePath);
            }

            ByteArrayContent data = new ByteArrayContent(fileData);

            MultipartFormDataContent multiContent = new MultipartFormDataContent
            {
               { data, discrepancyFileVM.Id.ToString(), "0" }
            };

            multiContent.Add(new StringContent(discrepancyFileVM.Id.ToString()), "Id");
            multiContent.Add(new StringContent(discrepancyFileVM.Name == null ? "" : discrepancyFileVM.Name), "Name");
            multiContent.Add(new StringContent(discrepancyFileVM.DisplayName), "DisplayName");
            multiContent.Add(new StringContent(discrepancyFileVM.DiscrepancyId.ToString()), "DiscrepancyId");

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DiscrepancyFileService.UploadDocumentAsync(dependecyParams, multiContent);
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            isBusySubmitButton = false;

            CloseDialog(true);
        }

        async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            isFileUploadHasError = false;
            isFileAdded = false;
            discrepancyFileVM.DisplayName = "";
            selectedFiles = e.GetMultipleFiles();
            ChangeLoaderVisibilityAction(true);

            foreach (IBrowserFile file in selectedFiles)
            {
                try
                {
                    if (file.Size > maxFileSize)
                    {
                        errorMessage = $"File size exceeds maximum limit {maxSizeInMB} MB.";

                        globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, errorMessage);
                        isFileUploadHasError = true;
                        ChangeLoaderVisibilityAction(true);
                        this.StateHasChanged();
                        return;
                    }

                    if(!supportedImagesFormats.Contains(Path.GetExtension(file.Name)))
                    {
                        errorMessage = $"Invalid file type.";

                        globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, errorMessage);
                        isFileUploadHasError = true;
                        ChangeLoaderVisibilityAction(false);
                        return;
                    }
                    
                    errorMessage = "";

                    Stream stream = file.OpenReadStream(maxFileSize);
                    uploadedFilePath = Path.GetFullPath($"{UploadDirectories.RootDirectory}\\{UploadDirectories.TempDocument}\\") + file.Name;

                    FileStream fs = File.Create(uploadedFilePath);

                    await stream.CopyToAsync(fs);
                    stream.Close();
                    fs.Close();

                    byte[] fileData = File.ReadAllBytes(uploadedFilePath);
                    discrepancyFileVM.DisplayName = Path.GetFileNameWithoutExtension(uploadedFilePath);
                    discrepancyFileVM.Name = Path.GetFileName(uploadedFilePath);
                    isFileAdded = true;
                }
                catch (Exception exc)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, exc.ToString());
                }
            }

            ChangeLoaderVisibilityAction(false);
            this.StateHasChanged();
        }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }
    }
}
