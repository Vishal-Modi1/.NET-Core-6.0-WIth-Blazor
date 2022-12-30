using DataModels.Enums;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;
using Web.UI.Models.Shared;
using Web.UI.Utilities;

namespace Web.UI.Shared
{
    public class BaseClass : ComponentBase
    {
        public bool isFilterBarVisible;

        [CascadingParameter]
        public Action<bool> ChangeLoaderVisibilityAction { get; set; }

        public bool isBusyAddButton, isBusyEditButton, isBusyDeleteButton, isBusySubmitButton;
        public string searchText;
        public string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        public int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        public List<int?> pageSizeOptions = Configuration.ConfigurationSettings.Instance.GridPagesizeOptions.ToList();

        public long maxProfileImageUploadSize = Configuration.ConfigurationSettings.Instance.MaxProfileImageSize;

        List<string> supportedDocumentsFormat = Configuration.ConfigurationSettings.Instance.SupportedDocuments.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        public string supportedImagesFormats = Configuration.ConfigurationSettings.Instance.SupportedImageTypes;
        public bool isLeftBarVisible { get; set; } = true;

        public bool isDisplayPopup { get; set; }
        public bool isDisplayChildPopup { get; set; }
        public string popupTitle { get; set; }
        public string childPopupTitle { get; set; }

        public OperationType operationType = OperationType.Create;

        public bool isGridDataLoading { get; set; }

        [CascadingParameter] public Task<AuthenticationState> AuthStat { get; set; }
        [CascadingParameter] public GlobalMembers globalMembers { get; set; }
    
        public CurrentUserPermissionManager _currentUserPermissionManager;

        public IEnumerable<FilterPanel> FilterPanelData { get; set; } = new List<FilterPanel>() { new FilterPanel() { Id = 0, Text = "Filters" } };

        public void OnSearchValueChanges<TypeOfValue>(string selectedValue, TelerikGrid<TypeOfValue> grid) where TypeOfValue : class
        {
            if (searchText != selectedValue)
            {
                grid.Rebind();
                searchText = selectedValue;
            }
        }

        public void PageSizeChangedHandler(int newPageSize)
        {
            pageSize = newPageSize;
        }

        public void SetSelectedMenuItem(string moduleName)
        {
            if (globalMembers.MenuItems != null)
            {
                globalMembers.SelectedItem = globalMembers.MenuItems.Where(x => x.Name == moduleName).FirstOrDefault();
            }
        }

        public bool ToggleLeftPane()
        {
            isLeftBarVisible = !isLeftBarVisible;
            return isLeftBarVisible;
        }
    }
}
