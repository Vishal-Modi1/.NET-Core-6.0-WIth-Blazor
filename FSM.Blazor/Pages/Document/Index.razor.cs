using DataModels.VM;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Radzen.Blazor;
using DataModels.VM.Document;
using DataModels.VM.Common;
using FSM.Blazor.Extensions;
using Microsoft.JSInterop;
using System.Net;
using DataModels.Enums;
using DataModels.Constants;
using System.Net.Http.Headers;

namespace FSM.Blazor.Pages.Document
{
    partial class Index
    {
        #region Params

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        [CascadingParameter]
        protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter]
        public string ParentModuleName { get; set; }

        [Parameter]
        public int? CompanyIdParam { get; set; }

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<DocumentDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;
        private DotNetObjectReference<Index>? objRef;

        #endregion

        IList<DocumentDataVM> data;
        int count, ModuleId, CompanyId;
        bool isLoading, isBusyAddNewButton, isBusyDeleteButton;
        string searchText;
        string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        IEnumerable<int> pageSizeOptions = Configuration.ConfigurationSettings.Instance.BlazorGridPagesizeOptions;
        DocumentFilterVM documentFilterVM;
        string moduleName = "Document";
        private string? result;

        string[] imageFormats = new string[] { ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        string[] previewSupportedFormats = new string[] { ".pdf", ".txt", ".png", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".svg" };
        BaseComponent.BaseComponent baseComponent;
        HttpCaller httpCaller = new HttpCaller();

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);
            
          //  ValidateTokenAsync();

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            documentFilterVM = await DocumentService.GetFiltersAsync(dependecyParams);
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            args.OrderBy = args.OrderBy.Replace("@", "");
            DocumentDatatableParams datatableParams = new DocumentDatatableParams().Create(args, "DisplayName");

           
            datatableParams.ModuleId = ModuleId;

            if (!string.IsNullOrWhiteSpace(ParentModuleName) && ParentModuleName != "Company")
            {
                datatableParams.ModuleId = documentFilterVM.ModulesList.Where(p => p.Name == ParentModuleName).FirstOrDefault().Id;
            }

            if (ParentModuleName == "Company")
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

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            data = await DocumentService.ListAsync(dependecyParams, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;

            isLoading = false;
        }

        async Task DocumentCreateDialog(Guid? id, string title, bool isCreate)
        {
         //   ValidateTokenAsync();

            if (isCreate)
            {
                SetAddNewButtonState(true);
            }
            else
            {
                SetEditButtonState(id.Value, true);
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
            DocumentVM documentData = await DocumentService.GetDetailsAsync(dependecyParams, id == null ? Guid.Empty : id.Value);
            documentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(dependecyParams);

            string height = "420px";

            if (_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.Admin).Result)
            {
                height = "470";
                documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(dependecyParams, documentData.CompanyId);
            }

            if (_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.SuperAdmin).Result)
            {
                height = "520";
                documentData.CompniesList = await CompanyService.ListDropDownValues(dependecyParams);
            }

            if (isCreate)
            {
                SetAddNewButtonState(false);
            }
            else
            {
                SetEditButtonState(id.Value, false);
            }

            if (!string.IsNullOrWhiteSpace(ParentModuleName))
            {
                documentData.ModuleId = documentFilterVM.ModulesList.Where(p => p.Name == ParentModuleName).FirstOrDefault().Id;
                documentData.IsFromParentModule = true;
            }

            await DialogService.OpenAsync<Create>(title,
                  new Dictionary<string, object>() { { "documentData", documentData } },
                  new DialogOptions() { Width = "500px", Height = height });

            await grid.Reload();
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

            StateHasChanged();
        }

        async Task DeleteAsync(Guid id)
        {
            await SetDeleteButtonState(true);

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
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
                DialogService.Close(true);
                message = new NotificationMessage().Build(NotificationSeverity.Success, "Document Details", response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Document Details", response.Message);
                NotificationService.Notify(message);
            }

            await grid.Reload();
        }

        async Task CopyLinkToClipboard(string link)
        {
            var jsFile = await JSRunTime.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await jsFile.InvokeVoidAsync("copyTextToClipboard", link);

            NotificationMessage message = new NotificationMessage().Build(NotificationSeverity.Success, "Link copied to the clipboard", "");
            NotificationService.Notify(message);

            DialogService.Close();
        }

        [JSInvokable]
        public async void ManageDocumentDownloadResponse(string id)
        {
            DocumentDataVM documentDataVM = data.Where(p => p.Id == Guid.Parse(id)).First();

            documentDataVM.TotalDownloads += 1;

            DependecyParams dependecyParams = DependecyParamsCreator.Create(_httpClient, "", "", AuthenticationStateProvider);
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

        //private async Task ValidateTokenAsync()
        //{
        //    string token = _currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.AccessToken).Result;
        //    bool isValid = await TokenValidatorService.IsTokenValid(_httpClient, token);

        //    if (!isValid)
        //    {
        //        NavManager.NavigateTo("/Login?TokenExpired=true");
        //    }
        //}
    }
}
