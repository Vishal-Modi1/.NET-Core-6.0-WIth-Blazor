using DataModels.Enums;
using DataModels.VM.Document.DocumentDirectory;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;

namespace Web.UI.Pages.Document.DocumentDirectory
{
    partial class DocumentDirectoryLeftPanel
    {
        [Parameter] public EventCallback<long?> RefreshGridCallBack { get; set; }

        private List<DocumentDirectorySummaryVM> documentDirectories { get; set; } = new();
        private DocumentDirectoryVM _documentDirectoryVM { get; set; }

        protected override async Task OnInitializedAsync()
        {
            isFilterBarVisible = true;
            await LoadData();
        }

        public void RefreshGrid(long? id)
        {
            RefreshGridCallBack.InvokeAsync(id);
        }

        public async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            documentDirectories = await DocumentDirectoryService.ListWithCountByComapnyId(dependecyParams);

            ChangeLoaderVisibilityAction(false);
        }

        void OpenCreateDocumentDirectoryDialog(DocumentDirectorySummaryVM documentDirectorySummary)
        {
            isDisplayPopup = true;
            operationType = OperationType.Create;
            popupTitle = "Create Directory";

            _documentDirectoryVM = new DocumentDirectoryVM(); 
            _documentDirectoryVM.Id = documentDirectorySummary.DocumentDirectoryId.GetValueOrDefault();
            _documentDirectoryVM.Name = documentDirectorySummary.Name;
            _documentDirectoryVM.CompanyId = documentDirectorySummary.CompanyId;
        }

        async void CloseDialog(bool refreshList)
        {
            isDisplayPopup = false;

            if(refreshList)
            {
                await LoadData();
            }
        }
    }
}
