using DataModels.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Telerik.Blazor.Components;
using Web.UI.Utilities;

namespace Web.UI.Shared
{
    public class BaseClass : ComponentBase
    {
        public bool isDisplayLoader, isBusyAddButton, isBusyEditButton, isBusyDeleteButton, isBusySubmitButton;
        public string searchText;
        public string pagingSummaryFormat = Configuration.ConfigurationSettings.Instance.PagingSummaryFormat;
        public int pageSize = Configuration.ConfigurationSettings.Instance.BlazorGridDefaultPagesize;
        public List<int?> pageSizeOptions = Configuration.ConfigurationSettings.Instance.GridPagesizeOptions.ToList();

        public long maxProfileImageUploadSize = Configuration.ConfigurationSettings.Instance.MaxProfileImageSize;

        List<string> supportedDocumentsFormat = Configuration.ConfigurationSettings.Instance.SupportedDocuments.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        public string supportedImagesFormat = Configuration.ConfigurationSettings.Instance.SupportedImageTypes;

        public bool isDisplayPopup { get; set; }
        public bool isDisplayChildPopup { get; set; }
        public string popupTitle { get; set; }
        public string childPopupTitle { get; set; }

        public OperationType operationType = OperationType.Create;

        public bool isGridDataLoading { get; set; }

        [CascadingParameter] public Task<AuthenticationState> AuthStat { get; set; }
        [CascadingParameter] public UINotification uiNotification { get; set; }
        public CurrentUserPermissionManager _currentUserPermissionManager;

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
    }
}
