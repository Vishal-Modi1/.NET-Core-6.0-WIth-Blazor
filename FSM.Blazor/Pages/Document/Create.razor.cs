using Configuration;
using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Document;
using FSM.Blazor.Extensions;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FSM.Blazor.Pages.Document
{
    partial class Create
    {
        [Parameter] public DocumentVM DocumentData { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        RadzenAutoComplete autoComplete;

        public bool isBusy, isLoading, isDisplayLoader;
        string uploadedFilePath = "";
        public long userId = long.MaxValue;

        IReadOnlyList<IBrowserFile> selectedFiles;

        public long maxFileSize = ConfigurationSettings.Instance.MaxDocumentUploadSize;
        string supportedDocumentsFormat = ConfigurationSettings.Instance.SupportedDocuments;
        long maxSizeInMB = 0;
        string errorMessage = "";
        List<string> selectedTagsList = new List<string>();
        RadzenTemplateForm<DocumentVM> form;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            string token = _currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result;

            if (DocumentData.UserId > 0)
            {
                userId = DocumentData.UserId;

                if (!string.IsNullOrWhiteSpace(DocumentData.Tags))
                {
                    selectedTagsList = DocumentData.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                OnChange(DocumentData.CompanyId);
            }

            maxSizeInMB = ConfigurationSettings.Instance.MaxDocumentUploadSize / (1024 * 1024);
            errorMessage = $"File size exceeds maximum limit {maxSizeInMB} MB.";

            base.OnInitialized();
        }

        async void OnChange(object companyId)
        {
            isDisplayLoader = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            DocumentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, DocumentData.CompanyId);

            isDisplayLoader = false;
            base.StateHasChanged();
        }

        public void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                string[] listTags = autoComplete.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);

                if (listTags.Length == 0)
                {
                    return;
                }

                foreach (string tag in listTags)
                {
                    if (!selectedTagsList.Contains(tag))
                    {
                        selectedTagsList.Add(tag);
                    }

                    autoComplete.Value = "";
                }

            }
        }

        private async Task UploadFilesAsync()
        {
            if (!form.EditContext.Validate())
            {
                return;
            }

            if ((selectedFiles == null || selectedFiles.Count() == 0) && DocumentData.Id == Guid.Empty)
            {
                await OpenErrorDialog("Please upload document.");
                return;
            }

            if (selectedFiles != null && selectedFiles.Count() > 0 && selectedFiles[0].Size > maxFileSize)
            {
                await OpenErrorDialog(errorMessage);
                return;
            }

            isLoading = true;
            SetSaveButtonState(true);

            if (DocumentData.UserId == 0 && userId == long.MaxValue)
            {
                DocumentData.UserId = userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
            }

            DocumentData.UserId = userId;

            byte[] fileData = new byte[0];

            if (!string.IsNullOrWhiteSpace(uploadedFilePath))
            {
                fileData = File.ReadAllBytes(uploadedFilePath);
            }

            ByteArrayContent data = new ByteArrayContent(fileData);

            MultipartFormDataContent multiContent = new MultipartFormDataContent
            {
               { data, DocumentData.Id.ToString(), DocumentData.CompanyId.ToString() }
            };

            string companyId = DocumentData.CompanyId == null ? "0" : DocumentData.CompanyId.ToString();

            multiContent.Add(new StringContent(DocumentData.Id.ToString()), "Id");
            multiContent.Add(new StringContent(companyId), "CompanyId");
            multiContent.Add(new StringContent(DocumentData.Name == null ? "" : DocumentData.Name), "Name");
            multiContent.Add(new StringContent(DocumentData.DisplayName), "DisplayName");
            multiContent.Add(new StringContent(DocumentData.ModuleId.ToString()), "ModuleId");
            multiContent.Add(new StringContent(DocumentData.UserId.ToString()), "UserId");
            multiContent.Add(new StringContent(DocumentData.Type), "Type");
            multiContent.Add(new StringContent(DocumentData.Size.ToString()), "Size");
            multiContent.Add(new StringContent(DocumentData.ExpirationDate.ToString()), "ExpirationDate");
            multiContent.Add(new StringContent(DocumentData.LastShareDate.ToString()), "LastShareDate");
            multiContent.Add(new StringContent(String.Join(",", selectedTagsList)), "Tags");
            multiContent.Add(new StringContent(DocumentData.IsShareable.ToString()), "IsShareable");

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentService.UploadDocumentAsync(dependecyParams, multiContent);

            SetSaveButtonState(false);

            ManageFileUploadResponse(response, "Document Details");
        }

        public void RemoveTag(string value)
        {
            selectedTagsList.Remove(value);
        }

        public void SubmitFormIgnore(KeyboardEventArgs e)
        {
            if (e.Code == "Enter")
            {
                return;
            }
        }

        private void ManageFileUploadResponse(CurrentResponse response, string summary)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong while uploading file!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(uploadedFilePath))
                {
                    File.Delete(uploadedFilePath);
                }

                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong while uploading file!", response.Message);
                NotificationService.Notify(message);
            }
        }

        async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            DocumentData.DisplayName = "";
            selectedFiles = e.GetMultipleFiles();

            foreach (IBrowserFile file in selectedFiles)
            {
                try
                {
                    if (file.Size > maxFileSize)
                    {
                        await OpenErrorDialog(errorMessage);
                        return;
                    }

                    Stream stream = file.OpenReadStream(maxFileSize);
                    uploadedFilePath = Path.GetFullPath($"{UploadDirectory.RootDirectory}\\{UploadDirectory.TempDocument}\\") + file.Name;

                    FileStream fs = File.Create(uploadedFilePath);

                    await stream.CopyToAsync(fs);
                    stream.Close();
                    fs.Close();

                    byte[] fileData = File.ReadAllBytes(uploadedFilePath);
                    DocumentData.Size = Convert.ToInt64(fileData.Length / 1024);
                    DocumentData.DisplayName = Path.GetFileName(uploadedFilePath);
                    DocumentData.Type = Path.GetExtension(uploadedFilePath).Substring(1);
                }
                catch (Exception exc)
                {

                }
            }

            this.StateHasChanged();
        }

        void OntestChange(object value)
        {
            var selectedTag = DocumentData.DocumentTagsList.Where(p => p.TagName == value).FirstOrDefault();

            if (selectedTag != null)
            {
                if (!selectedTagsList.Contains(selectedTag.TagName))
                {
                    selectedTagsList.Add(selectedTag.TagName);
                }
                
                autoComplete.Value = "";
            }
        }

        public async Task OpenCreateTagDialogAsync()
        {
            await DialogService.OpenAsync<DocumentTag.Create>("Create",
                  null, new DialogOptions() { Width = "500px", Height = "300px" });

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            DocumentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(dependecyParams);
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
