﻿using DataModels.VM;
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

        [Inject]
        protected IMemoryCache memoryCache { get; set; }

        [CascadingParameter]
        public RadzenDataGrid<DocumentDataVM> grid { get; set; }

        [Inject]
        NotificationService NotificationService { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

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

        protected override async Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(memoryCache);

            if (!_currentUserPermissionManager.IsAllowed(AuthStat, DataModels.Enums.PermissionType.View, moduleName))
            {
                NavManager.NavigateTo("/Dashboard");
            }

            documentFilterVM = await DocumentService.GetFiltersAsync(_httpClient);
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            args.OrderBy = args.OrderBy.Replace("@", "");
            DocumentDatatableParams datatableParams = new DocumentDatatableParams().Create(args, "DisplayName");

            datatableParams.CompanyId = CompanyId;
            datatableParams.ModuleId = ModuleId;

            if (!string.IsNullOrWhiteSpace(ParentModuleName))
            {
                datatableParams.ModuleId = documentFilterVM.ModulesList.Where(p => p.Name == ParentModuleName).FirstOrDefault().Id;
            }

            datatableParams.UserRole = await _currentUserPermissionManager.GetRole(AuthStat);
            datatableParams.SearchText = searchText;
            pageSize = datatableParams.Length;

            data = await DocumentService.ListAsync(_httpClient, datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;

            isLoading = false;
        }

        async Task DocumentCreateDialog(Guid? id, string title)
        {
            SetAddNewButtonState(true);

            DocumentVM documentData = await DocumentService.GetDetailsAsync(_httpClient, id == null ? Guid.Empty : id.Value);
            documentData.DocumentTagsList = await DocumentService.GetDocumentTagsList(_httpClient);
            
            string height = "420px";

            if (_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.Admin).Result)
            {
                height = "470";
                documentData.UsersList = await UserService.ListDropDownValuesByCompanyId(_httpClient, documentData.CompanyId);
            }

            if (_currentUserPermissionManager.IsValidUser(AuthStat, UserRole.SuperAdmin).Result)
            {
                height = "520";
                documentData.CompniesList = await CompanyService.ListDropDownValues(_httpClient);
            }

            SetAddNewButtonState(false);

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

        async Task DeleteAsync(Guid id)
        {
            await SetDeleteButtonState(true);

            CurrentResponse response = await DocumentService.DeleteAsync(_httpClient, id);

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

        async Task DownloadDocument(Guid id)
        {
            DocumentDataVM documentDataVM = data.Where(p => p.Id == id).First();

            Stream fileStream = GetFileStream(documentDataVM.DocumentPath);

            using var streamRef = new DotNetStreamReference(stream: fileStream);

            var jsFile = await JS.InvokeAsync<IJSObjectReference>("import", "/js/auth.js");
            await jsFile.InvokeVoidAsync("downloadFileFromStream", documentDataVM.DisplayName, streamRef);
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
    }
}