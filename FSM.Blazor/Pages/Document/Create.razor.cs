using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Document;
using FSM.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;
using Radzen;
using DE = DataModels.Entities;
using Configuration;
using FSM.Blazor.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Web;
using FSM.Blazor.Data.Common;

namespace FSM.Blazor.Pages.Document
{
    partial class Create
    {
        [Parameter]
        public DocumentVM documentData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

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
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
            string token = _currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result;
            bool isValid = await TokenValidatorService.IsTokenValid(_httpClient, token);

            if (!isValid)
            {
                NavigationManager.NavigateTo("/Login?TokenExpired=true");
            }

            if (documentData.UserId > 0)
            {
                userId = documentData.UserId;

                if (!string.IsNullOrWhiteSpace(documentData.Tags))
                {
                    selectedTagsList = documentData.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                OnChange(documentData.CompanyId);
            }


            maxSizeInMB = ConfigurationSettings.Instance.MaxDocumentUploadSize / (1024 * 1024);
            errorMessage = $"File size exceeds maximum limit {maxSizeInMB} MB.";

            base.OnInitialized();
        }

        async void OnChange(object companyId)
        {
            isDisplayLoader = true;

            documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(_httpClient, documentData.CompanyId);

            isDisplayLoader = false;
            base.StateHasChanged();
        }

        //public async Task Submit()
        //{
        //    if ((selectedFiles == null || selectedFiles.Count() == 0) && documentData.Id == Guid.Empty)
        //    {
        //        await OpenErrorDialog("Please upload document.");
        //        return;
        //    }

        //    if (selectedFiles != null && selectedFiles.Count() > 0 && selectedFiles[0].Size > maxFileSize)
        //    {
        //        await OpenErrorDialog(errorMessage);
        //        return;
        //    }

        //    isLoading = true;
        //    SetSaveButtonState(true);

        //    if (documentData.UserId == 0 && userId == long.MaxValue)
        //    {
        //        documentData.UserId = userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
        //    }

        //    documentData.UserId = userId;

        //    CurrentResponse response = await DocumentService.SaveandUpdateAsync(_httpClient, documentData);

        //    SetSaveButtonState(false);

        //    NotificationMessage message;

        //    if (response == null)
        //    {
        //        message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
        //        NotificationService.Notify(message);
        //    }
        //    else if (((int)response.Status) == 200)
        //    {
        //        if (response != null && response.Status == System.Net.HttpStatusCode.OK)
        //        {
        //            if (selectedFiles != null && selectedFiles.Count() > 0)
        //            {
        //                var data = JsonConvert.DeserializeObject<DE.Document>(response.Data.ToString());

        //                documentData.Id = data.Id;
        //                documentData.CompanyId = data.CompanyId;

        //                await UploadFilesAsync();
        //            }
        //            else
        //            {
        //                DialogService.Close(true);
        //                message = new NotificationMessage().Build(NotificationSeverity.Success, "Document Details", response.Message);
        //                NotificationService.Notify(message);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        message = new NotificationMessage().Build(NotificationSeverity.Error, "Document Details", response.Message);
        //        NotificationService.Notify(message);
        //    }

        //    isLoading = false;
        //}

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

            if ((selectedFiles == null || selectedFiles.Count() == 0) && documentData.Id == Guid.Empty)
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

            if (documentData.UserId == 0 && userId == long.MaxValue)
            {
                documentData.UserId = userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
            }

            documentData.UserId = userId;

            byte[] fileData = new byte[0];

            if (!string.IsNullOrWhiteSpace(uploadedFilePath))
            {
                fileData = File.ReadAllBytes(uploadedFilePath);
            }

            ByteArrayContent data = new ByteArrayContent(fileData);

            MultipartFormDataContent multiContent = new MultipartFormDataContent
            {
               { data, documentData.Id.ToString(), documentData.CompanyId.ToString() }
            };

            string companyId = documentData.CompanyId == null ? "0" : documentData.CompanyId.ToString();

            multiContent.Add(new StringContent(documentData.Id.ToString()), "Id");
            multiContent.Add(new StringContent(companyId), "CompanyId");
            multiContent.Add(new StringContent(documentData.Name == null ? "" : documentData.Name), "Name");
            multiContent.Add(new StringContent(documentData.DisplayName), "DisplayName");
            multiContent.Add(new StringContent(documentData.ModuleId.ToString()), "ModuleId");
            multiContent.Add(new StringContent(documentData.UserId.ToString()), "UserId");
            multiContent.Add(new StringContent(documentData.Type), "Type");
            multiContent.Add(new StringContent(documentData.Size.ToString()), "Size");
            multiContent.Add(new StringContent(documentData.ExpirationDate.ToString()), "ExpirationDate");
            multiContent.Add(new StringContent(documentData.LastShareDate.ToString()), "LastShareDate");
            multiContent.Add(new StringContent(String.Join(",", selectedTagsList)), "Tags");
            multiContent.Add(new StringContent(documentData.IsShareable.ToString()), "IsShareable");

            CurrentResponse response = await DocumentService.UploadDocumentAsync(_httpClient, multiContent);

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

                DialogService.Close(true);
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
            documentData.DisplayName = "";
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
                    documentData.Size = Convert.ToInt64(fileData.Length / 1024);
                    documentData.DisplayName = Path.GetFileName(uploadedFilePath);
                    documentData.Type = Path.GetExtension(uploadedFilePath).Substring(1);
                }
                catch (Exception exc)
                {

                }
            }

            this.StateHasChanged();
        }

        void OntestChange(object value)
        {
            var selectedTag = documentData.DocumentTagsList.Where(p => p.TagName == value).FirstOrDefault();

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

            documentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(_httpClient);
        }

        private void SetSaveButtonState(bool isBusyState)
        {
            isBusy = isBusyState;
            StateHasChanged();
        }
    }
}
