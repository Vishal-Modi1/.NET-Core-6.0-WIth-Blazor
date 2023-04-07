using Web.UI.Models.Document;
using DataModels.VM.Common;
using Web.UI.Pages.Document.DocumentTag;
using Web.UI.Utilities;

namespace Web.UI.Pages.Document
{
    partial class Index
    {
        DocumentsList documentsList;
        LeftPanel leftPanel;
        public List<DropDownValues> companies = new();
        public int companyId ;

        protected override async Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companies = await CompanyService.ListDropDownValues(dependecyParams);

            companyId = globalMembers.CompanyId;
        }

        public void RefreshDocumentsGrid(TagFilterParamteres tagFilterParamteres)
        {
            documentsList.RefreshGrid(tagFilterParamteres);
        }

        private async Task OnCompanyValueChanges(int selectedValue)
        {
            if (companyId != selectedValue)
            {
                companyId = selectedValue;
                await documentsList.GetDataOnCompanyValueChange(companyId);
                leftPanel.documentTagsList = await DocumentTagService.ListByCompanyId(dependecyParams, companyId);
                leftPanel.documentTagsList.ForEach(p => { p.IsSelected = true; });

                StateHasChanged();
            }
        }
    }
}
