using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Document;
using DataModels.VM.Common;
using Web.UI.Extensions;
using Microsoft.JSInterop;
using System.Net;
using DataModels.Enums;
using Telerik.Blazor.Components;

namespace Web.UI.Pages.Document
{
    partial class Index
    {
        #region Params

        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyIdParam { get; set; }
        [CascadingParameter] public TelerikGrid<DocumentDataVM> grid { get; set; }
       
        private DotNetObjectReference<Index>? objRef;

        #endregion

        IList<DocumentDataVM> data;
        DocumentFilterVM documentFilterVM;
        string moduleName = "Document", documentPath;
        private string? result;
        bool isImageType;

        string[] imageFormats = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        string[] previewSupportedFormats = new string[] { ".pdf", ".txt", ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        DocumentVM documentData;
        DocumentDataVM documentDataVM;

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            documentFilterVM = new DocumentFilterVM();
            //  ValidateTokenAsync();

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentFilterVM = await DocumentService.GetFiltersAsync(dependecyParams);
        }

        async Task LoadData(GridReadEventArgs args)
        {
            DocumentDatatableParams datatableParams = new DatatableParams().Create(args, "DisplayName").Cast<DocumentDatatableParams>();

            datatableParams.ModuleId = documentFilterVM.ModuleId;

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
                datatableParams.CompanyId = documentFilterVM.CompanyId;
            }

            datatableParams.UserRole = await _currentUserPermissionManager.GetRole(AuthStat);
            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            data = await DocumentService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;
        }

        private void OnCompanyValueChanges(int selectedValue)
        {
            if (documentFilterVM.CompanyId != selectedValue)
            {
                grid.Rebind();
                documentFilterVM.CompanyId = selectedValue;
            }
        }

        private void OnModuleValueChanges(int selectedValue)
        {
            if (documentFilterVM.ModuleId != selectedValue)
            {
                grid.Rebind();
                documentFilterVM.ModuleId = selectedValue;
            }
        }

        async Task DocumentCreateDialog(DocumentDataVM documentDataVM)
        {
            if (documentDataVM.Id == Guid.Empty)
            {
                operationType = OperationType.Create;
                isBusyAddButton = true;
                popupTitle = "Upload New Document";
            }
            else
            {
                operationType = OperationType.Edit;
                documentDataVM.IsLoadingEditButton = true;
                popupTitle = "Update Document";
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentData = await DocumentService.GetDetailsAsync(dependecyParams, documentDataVM.Id == Guid.Empty ? Guid.Empty : documentDataVM.Id);
            documentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(dependecyParams);

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

            if (documentDataVM.Id == Guid.Empty)
            {
                isBusyAddButton = false;
            }
            else
            {
                documentDataVM.IsLoadingEditButton = false;
            }

            isDisplayPopup = true;
        }

        async Task DeleteAsync(Guid id)
        {
            isBusyDeleteButton = true;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await DocumentService.DeleteAsync(dependecyParams, id);

            isBusyDeleteButton = false;

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                await CloseDialog(true);
            }
            else
            {
                await CloseDialog(false);
            }
        }

        async Task CopyLinkToClipboard(string link)
        {
            popupTitle = "Share Document";

            var jsFile = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await jsFile.InvokeVoidAsync("copyTextToClipboard", link);

            uiNotification.DisplaySuccessNotification(uiNotification.Instance, "Link copied to the clipboard");
            await CloseDialog(false);
        }

        [JSInvokable]
        public async void ManageDocumentDownloadResponse(string id)
        {
            DocumentDataVM documentDataVM = data.Where(p => p.Id == Guid.Parse(id)).First();
            documentDataVM.TotalDownloads += 1;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            await DocumentService.UpdateTotalDownloadsAsync(dependecyParams, Guid.Parse(id));
        }

        async Task DownloadDocument(DocumentDataVM documentDataVM)
        {
            documentDataVM.IsLoadingDownloadButton = true;

            Stream fileStream = GetFileStream(documentDataVM.DocumentPath);
            using var streamRef = new DotNetStreamReference(stream: fileStream);
            var jsFile = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");

            string fileName = documentDataVM.DisplayName;
            bool hasExtension = Path.HasExtension(documentDataVM.DisplayName);

            if (!hasExtension)
            {
                fileName += "." + documentDataVM.Type;
            }

            await jsFile.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef, documentDataVM.Id.ToString());

            objRef = DotNetObjectReference.Create(this);
            result = await jsFile.InvokeAsync<string>("ManageDocumentDownloadResponse", objRef, documentDataVM.Id.ToString());
            
            documentDataVM.IsLoadingDownloadButton = false;
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

        async Task CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
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
            popupTitle = "Share Document";

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
