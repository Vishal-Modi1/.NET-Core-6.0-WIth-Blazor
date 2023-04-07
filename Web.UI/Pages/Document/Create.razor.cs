using Configuration;
using DataModels.Constants;
using DataModels.VM.Common;
using DataModels.VM.Document;
using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Telerik.Blazor.Components;
using DataModels.Enums;
using DataModels.VM.Scheduler;

namespace Web.UI.Pages.Document
{
    partial class Create
    {
        [Parameter] public DocumentVM documentData { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        [Parameter] public string ParentModuleName { get; set; }

        DocumentTagVM _documentTagVM { get; set; }
        List<long> selectedTags = new List<long>();

        string uploadedFilePath = "";

        IReadOnlyList<IBrowserFile> selectedFiles;
        public string SaveUrl => ToAbsoluteUrl("api/fileupload/save");

        public long maxFileSize = ConfigurationSettings.Instance.MaxDocumentUploadSize;
        string supportedDocumentsFormat = ConfigurationSettings.Instance.SupportedDocuments;
        int maxSizeInMB = 1;
        public EditContext editContext { get; set; }
        string errorMessage;
        bool isFileUploadHasError, isFileAdded;
        bool isDisplayChildPopup = false;

        //RadzenTemplateForm<DocumentVM> form;

        public string ToAbsoluteUrl(string url)
        {
            return $"{NavigationManager.BaseUri}{url}";
        }

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            string token = _currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            //selectedTags = documentData.DocumentVsDocumentTags.Select(p => Convert.ToInt64(p.DocumentTagId)).ToList();

            if (!string.IsNullOrWhiteSpace(documentData.Tags))
            {
                selectedTags = documentData.Tags.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => Convert.ToInt64(p)).ToList();
            }

            editContext = new EditContext(documentData);

            maxSizeInMB = (int)ConfigurationSettings.Instance.MaxDocumentUploadSize / (1024 * 1024);
            errorMessage = $"File size exceeds maximum limit {maxSizeInMB} MB.";

            isFileAdded = !string.IsNullOrWhiteSpace(documentData.DisplayName);
            base.OnInitialized();
        }

        void UpdateSelectedDocumentTagData(List<long> selectedData)
        {
            selectedTags = selectedData;
        }

        async void OnChange(int value)
        {
            documentData.CompanyId = value;
            ChangeLoaderVisibilityAction(true);

            documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);
            documentData.DocumentTagsList = await DocumentTagService.ListDropdownValues(dependecyParams, documentData.CompanyId);
            //    documentData.DocumentDirectoriesList = await DocumentDirectoryService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);

            ChangeLoaderVisibilityAction(false);
            base.StateHasChanged();
        }

        private async Task UploadFilesAsync()
        {
            isFileUploadHasError = false;

            if (!editContext.Validate())
            {
                return;
            }

            if ((selectedFiles == null || selectedFiles.Count() == 0) && documentData.Id == Guid.Empty)
            {
                isFileUploadHasError = true;
                errorMessage = "Please upload document";
                return;
            }

            if (selectedFiles != null && selectedFiles.Count() > 0 && selectedFiles[0].Size > maxFileSize)
            {
                isFileUploadHasError = true;
                return;
            }

            isBusySubmitButton = true;

            //if (documentData.UserId == 0 && userId == long.MaxValue)
            //{
            //    documentData.UserId = userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
            //}

            //documentData.UserId = userId;
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
            multiContent.Add(new StringContent(documentData.DocumentDirectoryId == null ? "" : documentData.DocumentDirectoryId.ToString()), "DirectoryId");
            multiContent.Add(new StringContent(documentData.Type), "Type");
            multiContent.Add(new StringContent(documentData.AircraftId.GetValueOrDefault().ToString()), "AircraftId");
            multiContent.Add(new StringContent(documentData.IsPersonalDocument.ToString()), "IsPersonalDocument");
            multiContent.Add(new StringContent(documentData.Size.ToString()), "Size");
            multiContent.Add(new StringContent(documentData.ExpirationDate.ToString()), "ExpirationDate");
            multiContent.Add(new StringContent(documentData.LastShareDate.ToString()), "LastShareDate");
            multiContent.Add(new StringContent(documentData.CreatedBy.ToString()), "CreatedBy");

            if (selectedTags != null)
            {
                multiContent.Add(new StringContent(String.Join(",", selectedTags)), "Tags");
            }

            multiContent.Add(new StringContent(documentData.IsShareable.ToString()), "IsShareable");

            CurrentResponse response = await DocumentService.UploadDocumentAsync(dependecyParams, multiContent);

            isBusySubmitButton = false;

            ManageFileUploadResponse(response, "Document Details");
        }

        private void ManageFileUploadResponse(CurrentResponse response, string summary)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

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
            isFileUploadHasError = false;
            isFileAdded = false;
            documentData.DisplayName = "";
            selectedFiles = e.GetMultipleFiles();

            try
            {
                ChangeLoaderVisibilityAction(true);

                string fileType = Path.GetExtension(e.File.Name);
                if (!supportedDocumentsFormat.Contains(fileType))
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "File type is not supported");
                    isFileUploadHasError = false;
                    ChangeLoaderVisibilityAction(false);
                    return;
                }

                if (e.File.Size > maxFileSize)
                {
                    errorMessage = $"File size exceeds maximum limit {maxSizeInMB} MB.";
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, errorMessage);
                    isFileUploadHasError = false;
                    ChangeLoaderVisibilityAction(false);
                    return;
                }

                errorMessage = "";

                Stream stream = e.File.OpenReadStream(maxFileSize);
                uploadedFilePath = Path.GetFullPath($"{UploadDirectories.RootDirectory}\\{UploadDirectories.TempDocument}\\") + e.File.Name;

                FileStream fs = File.Create(uploadedFilePath);

                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();

                byte[] fileData = File.ReadAllBytes(uploadedFilePath);
                documentData.Size = Convert.ToInt64(fileData.Length / 1024);
                documentData.DisplayName = Path.GetFileName(uploadedFilePath);
                documentData.Type = Path.GetExtension(uploadedFilePath).Substring(1);

                isFileAdded = true;
            }
            catch (Exception ex)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, ex.ToString());
            }

            ChangeLoaderVisibilityAction(false);

        }

        public async Task OpenCreateTagDialogAsync()
        {
            ChangeLoaderVisibilityAction(true);

            operationType = OperationType.Create;
            childPopupTitle = "Create Tag";

            _documentTagVM = new DocumentTagVM();

            if (globalMembers.IsSuperAdmin)
            {
                _documentTagVM.CompniesList = await CompanyService.ListDropDownValues(dependecyParams);
            }
            else
            {
                _documentTagVM.CompanyId = globalMembers.CompanyId;
            }

            isDisplayChildPopup = true;
            ChangeLoaderVisibilityAction(false);

            base.StateHasChanged();
        }

        public void CloseDialog(bool reloadGrid)
        {
            CloseDialogCallBack.InvokeAsync(reloadGrid);
        }

        public async Task CloseChildDialog(bool reloaList)
        {
            isDisplayChildPopup = false;

            if (reloaList)
            {
                documentData.DocumentTagsList = await DocumentTagService.ListDropdownValues(dependecyParams, documentData.CompanyId);
            }
        }
    }
}
