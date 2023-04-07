using Web.UI.Utilities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Document;
using DataModels.VM.Common;
using Microsoft.JSInterop;
using System.Net;
using DataModels.Enums;
using Telerik.Blazor.Components;
using DataModels.Constants;
using Web.UI.Pages.Document.DocumentDirectory;
using Web.UI.Extensions;
using Utilities;
using Web.UI.Models.Document;

namespace Web.UI.Pages.Document
{
    partial class DocumentsList
    {
        #region Params

        [Parameter] public string ParentModuleName { get; set; }
        [Parameter] public int? CompanyIdParam { get; set; }
        [Parameter] public long? AircraftIdParam { get; set; }
        [Parameter] public bool? IsPersonalDocument { get; set; }
        [CascadingParameter(Name = "IsTagsLoaded")] protected bool IsTagsLoaded { get; set; }
        [CascadingParameter] public TelerikGrid<DocumentDataVM> grid { get; set; }

        private DotNetObjectReference<DocumentsList>? objRef;

        #endregion

        IList<DocumentDataVM> data;
        DocumentFilterVM documentFilterVM;
        string moduleName = "Document", documentPath;
        bool isImageType;

        private TagFilterParamteres _tagFilterParamteres = new TagFilterParamteres();

        string[] imageFormats = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        string[] previewSupportedFormats = new string[] { ".pdf", ".txt", ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        DocumentVM documentData;
        DocumentDataVM documentDataVM;

        DocumentDatatableParams datatableParams;
        int total = 0;
        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            documentFilterVM = new DocumentFilterVM();

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavigationManager.NavigateTo("/Dashboard");
            }

            SetSelectedMenuItem("Document");

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentFilterVM = await DocumentService.GetFiltersAsync(dependecyParams);

            if (globalMembers.IsSuperAdmin && ParentModuleName == Module.Company.ToString())
            {
                documentFilterVM.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, CompanyIdParam.Value);
            }
        }

        async Task LoadData(GridReadEventArgs args)
        {
            isGridDataLoading = true;

            datatableParams = new DatatableParams().Create(args, "DisplayName").Cast<DocumentDatatableParams>();
            SetFiltersValue();

            data = await DocumentService.ListAsync(dependecyParams, datatableParams);
            args.Total = data.Count() > 0 ? data[0].TotalRecords : 0;
            args.Data = data;

            total = args.Total;

            data.ToList().ForEach(p =>
            {
                p.CreatedOn = DateConverter.ToLocal(p.CreatedOn, globalMembers.Timezone);
            });

            isGridDataLoading = false;
        }

        private void SetFiltersValue()
        {
            datatableParams.ModuleId = documentFilterVM.ModuleId;
            datatableParams.AircraftId = AircraftIdParam;
            datatableParams.IsFromMyProfile = IsPersonalDocument.GetValueOrDefault();
            datatableParams.CompanyId = documentFilterVM.CompanyId;

            if (!string.IsNullOrWhiteSpace(ParentModuleName))
            {
                if (ParentModuleName == Module.Company.ToString())
                {
                    datatableParams.CompanyId = CompanyIdParam.GetValueOrDefault();
                }
                else if (ParentModuleName == Module.MyProfile.ToString())
                {
                    datatableParams.ModuleId = 0;
                }
                else if (ParentModuleName != Module.Company.ToString())
                {
                    datatableParams.ModuleId = (int)((Module)Enum.Parse(typeof(Module), ParentModuleName));
                }
            }

            datatableParams.UserId = documentFilterVM.UserId;
            datatableParams.DocumentType = documentFilterVM.DocumentType;

            datatableParams.UserRole = globalMembers.UserRole;
            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            datatableParams.TagIds = _tagFilterParamteres.TagIds;
            datatableParams.IncludeDocumentsWithoutTags = _tagFilterParamteres.IncludeDocumentsWithoutTags;

            datatableParams.IsIgnoreTagFilter = !IsTagsLoaded || _tagFilterParamteres.IsIgnoreTagFilter;
        }

        private async Task OnCompanyValueChange(int selectedValue)
        {
            if (documentFilterVM.CompanyId != selectedValue)
            {
                await GetDataOnCompanyValueChange(selectedValue);
            }
        }

        public async Task GetDataOnCompanyValueChange(int companyId)
        {
            ChangeLoaderVisibilityAction(true);

            documentFilterVM.CompanyId = companyId;
            documentFilterVM.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, documentFilterVM.CompanyId);
            grid.Rebind();

            ChangeLoaderVisibilityAction(false);
        }

        private void OnUserValueChanges(long selectedValue)
        {
            if (documentFilterVM.UserId != selectedValue)
            {
                documentFilterVM.UserId = selectedValue;
                grid.Rebind();
            }
        }

        private void OnTypeValueChanges(string selectedValue)
        {
            if (documentFilterVM.DocumentType != selectedValue)
            {
                documentFilterVM.DocumentType = selectedValue;
                grid.Rebind();
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

        async Task OpenDocumentCreateDialog(DocumentDataVM documentDataVM)
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

            documentData = await DocumentService.GetDetailsAsync(dependecyParams, documentDataVM.Id == Guid.Empty ? Guid.Empty : documentDataVM.Id);

            if (documentData != null && CompanyIdParam != null)
            {
                documentData.CompanyId = CompanyIdParam.Value;
                documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, CompanyIdParam.Value);
                documentData.DocumentTagsList = await DocumentTagService.ListDropdownValues(dependecyParams, CompanyIdParam.Value);
                //   documentData.DocumentDirectoriesList = await DocumentDirectoryService.ListDropDownValuesByCompanyId(dependecyParams, CompanyIdParam.Value);
            }

            if (globalMembers.IsAdmin)
            {
                documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);
                documentData.DocumentTagsList = await DocumentTagService.ListDropdownValues(dependecyParams, documentData.CompanyId);
                // documentData.DocumentDirectoriesList = await DocumentDirectoryService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);
            }
            else if (globalMembers.IsSuperAdmin)
            {
                documentData.CompniesList = await CompanyService.ListDropDownValues(dependecyParams);
                documentData.DocumentTagsList = await DocumentTagService.ListDropdownValues(dependecyParams, documentData.CompanyId);
            }
            else
            {
                var user = (await AuthStat).User;
                documentData.UserId = Convert.ToInt64(user.Claims.Where(c => c.Type == CustomClaimTypes.UserId).First().Value);
                documentData.DocumentTagsList = await DocumentTagService.ListDropdownValues(dependecyParams, documentData.CompanyId);
                //  documentData.DocumentDirectoriesList = await DocumentDirectoryService.ListDropDownValuesByCompanyId(dependecyParams, CompanyIdParam.Value);
            }

            if (!string.IsNullOrWhiteSpace(ParentModuleName))
            {
                documentData.ModuleId = (int)(Module)Enum.Parse(typeof(Module), ParentModuleName);
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

            documentData.AircraftId = AircraftIdParam;
            documentData.IsPersonalDocument = IsPersonalDocument.GetValueOrDefault();

            isDisplayPopup = true;
        }

        async Task DeleteAsync(Guid id)
        {
            isBusyDeleteButton = true;

            CurrentResponse response = await DocumentService.DeleteAsync(dependecyParams, id);

            isBusyDeleteButton = false;

            if (response.Status == HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
            else
            {
                CloseDialog(false);
            }
        }

        async Task CopyLinkToClipboard(string link)
        {
            popupTitle = "Share Document";

            var jsFile = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await jsFile.InvokeVoidAsync("copyTextToClipboard", link);

            globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, "Link copied to the clipboard");
            CloseDialog(false);
        }

        [JSInvokable]
        public async void ManageDocumentDownloadResponse(string id)
        {
            DocumentDataVM documentDataVM = data.Where(p => p.Id == Guid.Parse(id)).First();
            documentDataVM.TotalDownloads += 1;

            await DocumentService.UpdateTotalDownloadsAsync(dependecyParams, Guid.Parse(id));
        }

        async Task DownloadDocument(DocumentDataVM documentDataVM)
        {
            try
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
                string result = await jsFile.InvokeAsync<string>("ManageDocumentDownloadResponse", objRef, documentDataVM.Id.ToString());


            }
            catch (Exception ex)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, ex.ToString());
            }
            finally
            {
                documentDataVM.IsLoadingDownloadButton = false;
            }
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

        void CloseDialog(bool reloadGrid)
        {
            isDisplayPopup = false;

            if (reloadGrid)
            {
                grid.Rebind();
                // await lef.LoadData();
            }
        }

        void OpenDeleteDialog(DocumentDataVM documentInfo)
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
            await DocumentService.UpdateTotalSharesAsync(dependecyParams, documentDataVM.Id);
            documentDataVM.TotalShares += 1;

            operationType = OperationType.DocumentShare;
            isDisplayPopup = true;
        }

        void OpenDocumentViewer(string link, bool isImage)
        {
            documentPath = link;
            isImageType = isImage;

            operationType = OperationType.DocumentViewer;
            isDisplayPopup = true;
        }

        void OpenDocumentPreviewPopupAsync(string link)
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

        public void RefreshGrid(TagFilterParamteres tagFilterParamteres)
        {
            IsTagsLoaded = true;
            _tagFilterParamteres = tagFilterParamteres;
            grid.Rebind();
        }
    }
}
