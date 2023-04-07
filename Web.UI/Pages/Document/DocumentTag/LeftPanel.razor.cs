using DataModels.Enums;
using DataModels.VM.Common;
using DataModels.VM.Document;
using Microsoft.AspNetCore.Components;
using Web.UI.Models.Document;
using Web.UI.Utilities;

namespace Web.UI.Pages.Document.DocumentTag
{
    partial class LeftPanel
    {
        [Parameter] public EventCallback<TagFilterParamteres> RefreshGridCallBack { get; set; }
        [CascadingParameter(Name = "CompanyId")] public int CompanyId { get; set; }
        public List<DocumentTagDataVM> documentTagsList { get; set; } = new();
        public DocumentTagVM _documentTagVM { get; set; }

        public TagFilterParamteres tagFilterParamteres = new TagFilterParamteres();

        public bool includeDocumentsWithoutTags { get; set; } = true;
        string popupWidth = "400px";

        protected override async Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            isFilterBarVisible = true;
            await LoadData();
        }

        public async Task RefreshGrid()
        {
            tagFilterParamteres.TagIds = string.Join(",", documentTagsList.Where(p => p.IsSelected).Select(p => p.Id).ToList());
            await RefreshGridCallBack.InvokeAsync(tagFilterParamteres);
        }

        public async Task LoadData()
        {
            ChangeLoaderVisibilityAction(true);
            
            var existingSelectedValue = documentTagsList.Where((p) => p.IsSelected).Select((p) => p.Id).ToList();   
            documentTagsList = await DocumentTagService.ListByCompanyId(dependecyParams, CompanyId);

            if (existingSelectedValue.Any())
            {
                documentTagsList.Where(p=> existingSelectedValue.Contains(p.Id)).ToList().ForEach(p => { p.IsSelected = true; });
            }
            else
            {
                documentTagsList.ForEach(p => { p.IsSelected = true; });
            }

            ChangeLoaderVisibilityAction(false);
        }

        public async void OpenCreateDocumentTagDialog(DocumentTagDataVM selectedValue, bool isFromTagFilterPopup = false)
        {
            ChangeLoaderVisibilityAction(true);

            if (selectedValue.Id == 0)
            {
                operationType = OperationType.Create;

                if (isFromTagFilterPopup)
                {
                    childPopupTitle = "Create Tag";
                }
                else
                {
                    popupTitle = "Create Tag";
                }

                _documentTagVM = new DocumentTagVM();
            }
            else
            {
                if (isFromTagFilterPopup)
                {
                    childPopupTitle = "Update Tag";
                }
                else
                {
                    popupTitle = "Update Tag";
                }

                _documentTagVM = await DocumentTagService.FindById(dependecyParams, (int)selectedValue.Id);

                operationType = OperationType.Edit;
            }

            if (globalMembers.IsSuperAdmin)
            {
                _documentTagVM.CompniesList = await CompanyService.ListDropDownValues(dependecyParams);
            }
            else
            {
                _documentTagVM.CompanyId = globalMembers.CompanyId;
            }

            popupWidth = "400px";

            if (isFromTagFilterPopup)
            {
                isDisplayChildPopup = true;
            }
            else
            {
                isDisplayPopup = true;
            }

            ChangeLoaderVisibilityAction(false);

            base.StateHasChanged();
        }

        void OpenDocumentTagFilterDialog()
        {
            operationType = OperationType.DocumentTagFilter;
            popupTitle = "Filter";
            popupWidth = "1000px";
            isDisplayPopup = true;
        }

        public async Task CloseDialog(bool refreshList)
        {
            if (isDisplayChildPopup)
            {
                isDisplayChildPopup = false;
                OpenDocumentTagFilterDialog();
            }
            else if (isDisplayPopup)
            {
                isDisplayPopup = false;
            }

            if (refreshList)
            {
                await LoadData();
            }
        }

        public async Task CloseFilterDialog(bool refreshGrid)
        {
            isDisplayPopup = false;

            if (refreshGrid)
            {
                await RefreshGrid();
            }
        }

        public async Task CheckboxChangedAsync(ChangeEventArgs e, DocumentTagDataVM selectedValue, bool isFromModalPopup = false)
        {
            selectedValue.IsSelected = (bool)e.Value;

            if (isFromModalPopup)
                return;

            await RefreshGrid();
        }

        public async Task IncludeDocumentsWithoutTagsCheckboxChangedAsync(bool value, bool isFromModalPopup = false)
        {
            includeDocumentsWithoutTags = value;
            tagFilterParamteres.IncludeDocumentsWithoutTags = value;

            if (isFromModalPopup)
                return;

            await RefreshGrid();
        }

        public bool selectAllValue
        {
            get
            {
                tagFilterParamteres.IsIgnoreTagFilter = documentTagsList.All(eq => eq.IsSelected) && includeDocumentsWithoutTags;
                return tagFilterParamteres.IsIgnoreTagFilter;
            }
        }

        public async void ValueChanged(bool value, bool isFromModalPopup = false)
        {
            documentTagsList.ForEach(eq =>
            {
                eq.IsSelected = value;
            });

            includeDocumentsWithoutTags = value;
            tagFilterParamteres.IncludeDocumentsWithoutTags = value;

            if (isFromModalPopup)
                return;

            await RefreshGrid();
        }
    }
}
