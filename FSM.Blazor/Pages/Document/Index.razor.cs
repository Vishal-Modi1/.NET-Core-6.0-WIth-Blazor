using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen.Blazor;
using DataModels.VM.Document;
using DataModels.VM.Common;
using FSM.Blazor.Extensions;
using Microsoft.JSInterop;
using System.Net;
using DataModels.Enums;

namespace FSM.Blazor.Pages.Document
{
    partial class Index
    {
        #region Params

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter] public string ParentModuleName { get; set; }

        [Parameter] public int? CompanyIdParam { get; set; }

        [CascadingParameter] public RadzenDataGrid<DocumentDataVM> grid { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;
        private DotNetObjectReference<Index>? objRef;

        #endregion

        IList<DocumentDataVM> data;
        int count, ModuleId, CompanyId;
        bool isLoading, isBusyAddNewButton, isBusyDeleteButton, isDisplayPopup, isImageType;
        string searchText, popupTitle;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        DocumentFilterVM documentFilterVM;
        string moduleName = "Document", documentPath ;
        private string? result;

        string[] imageFormats = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        string[] previewSupportedFormats = new string[] { ".pdf", ".txt", ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        BaseComponent.BaseComponent baseComponent;
        HttpCaller httpCaller = new HttpCaller();
        OperationType operationType = OperationType.Create;
        DocumentVM documentData;
        DocumentDataVM documentDataVM;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);

            //  ValidateTokenAsync();

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentFilterVM = await DocumentService.GetFiltersAsync(dependecyParams);
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            args.OrderBy = args.OrderBy.Replace("@", "");
            DocumentDatatableParams datatableParams = new DocumentDatatableParams().Create(args, "DisplayName");

            datatableParams.ModuleId = ModuleId;

            if (!string.IsNullOrWhiteSpace(ParentModuleName) && ParentModuleName != Module.Company.ToString())
            {
                datatableParams.ModuleId = (int)((Module)Enum.Parse(typeof(Module), ParentModuleName));
            }

            if (ParentModuleName == Module.Company.ToString())
            {
                datatableParams.CompanyId = CompanyIdParam.GetValueOrDefault();
            }
            else
            {
                datatableParams.CompanyId = CompanyId;
            }

            datatableParams.UserRole = await _currentUserPermissionManager.GetRole(AuthStat);
            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await DocumentService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;

            isLoading = false;
        }

        async Task DocumentCreateDialog(Guid? id, string title, bool isCreate)
        {
            if (isCreate)
            {
                operationType = OperationType.Create;
                SetAddNewButtonState(true);
            }
            else
            {
                operationType = OperationType.Edit;
                SetEditButtonState(id.Value, true);
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentData = await DocumentService.GetDetailsAsync(dependecyParams, id == null ? Guid.Empty : id.Value);
          //  documentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(dependecyParams);

            if (_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.Admin).Result)
            {
                documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);
            }

            if (_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.SuperAdmin).Result)
            {
                documentData.CompniesList = await CompanyService.ListDropDownValues(dependecyParams);
            }

            if (!string.IsNullOrWhiteSpace(ParentModuleName))
            {
                documentData.ModuleId = (int)((Module)Enum.Parse(typeof(Module), ParentModuleName));
                documentData.IsFromParentModule = true;
            }

            if (isCreate)
            {
                SetAddNewButtonState(false);
            }
            else
            {
                SetEditButtonState(id.Value, false);
            }

            isDisplayPopup = true;
            popupTitle = title;
        }

        private void SetAddNewButtonState(bool isBusy)
        {
            isBusyAddNewButton = isBusy;
            StateHasChanged();
        }

        private void SetEditButtonState(Guid id, bool isBusy)
        {
            var details = data.Where(p => p.Id == id).First();
            details.IsLoadingEditButton = isBusy;
        }

        async Task DeleteAsync(Guid id)
        {
            await SetDeleteButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentService.DeleteAsync(dependecyParams, id);

            await SetDeleteButtonState(false);

            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                await CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Document Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Document Details", response.Message);
                NotificationService.Notify(message);
            }
        }

        async Task CopyLinkToClipboard(string link)
        {
            popupTitle = "Share Document";

            var jsFile = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await jsFile.InvokeVoidAsync("copyTextToClipboard", link);

            NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Success, "Link copied to the clipboard", "");
            NotificationService.Notify(message);

            await CloseDialog(true);
        }

        [JSInvokable]
        public async void ManageDocumentDownloadResponse(string id)
        {
            DocumentDataVM documentDataVM = data.Where(p => p.Id == Guid.Parse(id)).First();

            documentDataVM.TotalDownloads += 1;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            await DocumentService.UpdateTotalDownloadsAsync(dependecyParams, Guid.Parse(id));
        }

        async Task DownloadDocument(Guid id)
        {
            DocumentDataVM documentDataVM = data.Where(p => p.Id == id).First();

            Stream fileStream = GetFileStream(documentDataVM.DocumentPath);

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            var jsFile = await JS.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");

            string fileName = documentDataVM.DisplayName;
            bool hasExtension = Path.HasExtension(documentDataVM.DisplayName);

            if (!hasExtension)
            {
                fileName += "." + documentDataVM.Type;
            }

            await jsFile.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef, id.ToString());

            objRef = DotNetObjectReference.Create(this);
            result = await jsFile.InvokeAsync<string>("ManageDocumentDownloadResponse", objRef, id.ToString());
        }

        private Stream GetFileStream(string documentPath)
        {
            try
            {
                var ubuilder = new UriBuilder();
                ubuilder.Path = documentPath;
                var newURL = ubuilder.Uri.LocalPath;

                WebClient webClient = new WebClient();

                byte[] fileBytes = webClient.DownloadData(documentPath);

                var fileStream = new MemoryStream(fileBytes);

                return fileStream;
            }

            catch (Exception exc)
            {
                return null;
            }

        }

        private async Task SetDeleteButtonState(bool isBusy)
        {
            isBusyDeleteButton = isBusy;
            await InvokeAsync(() => StateHasChanged());
        }

        async Task CloseDialog(bool isCancelled)
        {
            isDisplayPopup = false;

            if (!isCancelled)
            {
                await grid.Reload();
            }
        }

        async Task OpenDeleteDialog(DocumentDataVM documentInfo)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Document";

            documentData = new DocumentVM();

            documentData.Id = documentInfo.Id;
            documentData.DisplayName = documentInfo.DisplayName;
        }

        async Task OpenShareDocumentDialog(Guid id)
        {
            documentDataVM = data.Where(p => p.Id == id).First();
            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            await DocumentService.UpdateTotalSharesAsync(dependecyParams, documentDataVM.Id);
            documentDataVM.TotalShares += 1;

            operationType = OperationType.DocumentShare;
            isDisplayPopup = true;
        }

        async void OpenDocumentViewer(string link, bool isImage)
        {
            documentPath = link;
            isImageType = isImage;

            operationType = OperationType.DocumentViewer;
            isDisplayPopup = true;
        }

        async void OpenDocumentPreviewPopupAsync(string link)
        {
            string extension = Path.GetExtension(link);

            if (previewSupportedFormats.Contains(extension))
            {
                popupTitle = "Document Viewer";
                operationType = OperationType.DocumentViewer;

                if (imageFormats.Contains(extension))
                {
                    OpenDocumentViewer(link, true);
                }
                else
                {
                    OpenDocumentViewer(link, false);
                }
            }
            else
            {
                popupTitle = "Document Viewer";
                operationType = 0;
            }

            isDisplayPopup = true;
        }

    }
}
