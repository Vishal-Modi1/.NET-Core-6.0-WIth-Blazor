using Configuration;
using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Document;
using Web.UI.Extensions;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.Document
{
    partial class Create
    {
        [Parameter] public DocumentVM documentData { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        //RadzenAutoComplete autoComplete;

        string uploadedFilePath = "";
        public long userId = long.MaxValue;

        IReadOnlyList<IBrowserFile> selectedFiles;
        public string SaveUrl => ToAbsoluteUrl("api/fileupload/save");

        public long maxFileSize = ConfigurationSettings.Instance.MaxDocumentUploadSize;
        List<string> supportedDocumentsFormat = ConfigurationSettings.Instance.SupportedDocuments.Split(new String[] { ","}, StringSplitOptions.RemoveEmptyEntries).ToList();
        int maxSizeInMB = 1;
        string errorMessage = "";
        List<string> selectedTagsList = new List<string>();
        //RadzenTemplateForm<DocumentVM> form;

        public string ToAbsoluteUrl(string url)
        {
            return $"{NavigationManager.BaseUri}{url}";
        }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            string token = _currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result;

            if (documentData.UserId > 0)
            {
                userId = documentData.UserId;

                if (!string.IsNullOrWhiteSpace(documentData.Tags))
                {
                    selectedTagsList = documentData.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }

               // OnChange(documentData.CompanyId);
            }

            maxSizeInMB = (int)ConfigurationSettings.Instance.MaxDocumentUploadSize / (1024 * 1024);
            errorMessage = $"File size exceeds maximum limit {maxSizeInMB} MB.";

            base.OnInitialized();
        }

        async void OnChange(int value)
        {
            documentData.CompanyId = value;
            isDisplayLoader = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);

            isDisplayLoader = false;
            base.StateHasChanged();
        }

        //public void Enter(KeyboardEventArgs e)
        //{
        //    if (e.Code == "Enter" || e.Code == "NumpadEnter")
        //    {
        //        string[] listTags = autoComplete.Value.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);

        //        if (listTags.Length == 0)
        //        {
        //            return;
        //        }

        //        foreach (string tag in listTags)
        //        {
        //            if (!selectedTagsList.Contains(tag))
        //            {
        //                selectedTagsList.Add(tag);
        //            }

        //            autoComplete.Value = "";
        //        }

        //    }
        //}

        private async Task UploadFilesAsync()
        {
            //if (!form.EditContext.Validate())
            //{
            //    return;
            //}

            if ((selectedFiles == null || selectedFiles.Count() == 0) && documentData.Id == Guid.Empty)
            {
                //await OpenErrorDialog("Please upload document.");
                return;
            }

            if (selectedFiles != null && selectedFiles.Count() > 0 && selectedFiles[0].Size > maxFileSize)
            {
                //await OpenErrorDialog(errorMessage);
                return;
            }

            isBusySubmitButton = true;

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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentService.UploadDocumentAsync(dependecyParams, multiContent);

            isBusySubmitButton = false;

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
            uiNotification.DisplayNotification(uiNotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (!string.IsNullOrWhiteSpace(uploadedFilePath))
                {
                    File.Delete(uploadedFilePath);
                }

                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
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
                       // await OpenErrorDialog(errorMessage);
                        return;
                    }

                    Stream stream = file.OpenReadStream(maxFileSize);
                    uploadedFilePath = Path.GetFullPath($"{UploadDirectories.RootDirectory}\\{UploadDirectories.TempDocument}\\") + file.Name;

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
                
                //autoComplete.Value = "";
            }
        }

        public async Task OpenCreateTagDialogAsync()
        {
            //await DialogService.OpenAsync<DocumentTag.Create>("Create",
            //      null, new DialogOptions() { Width = "500px", Height = "300px" });

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(dependecyParams);
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
