using DataModels.VM.Common;
using DataModels.VM.Company;
using FSM.Blazor.Data.Company;
using FSM.Blazor.Utilities;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FSM.Blazor.Pages.Company
{
    partial class Index
    {
        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        List<CompanyVM> companiesList = new List<CompanyVM>();

        IList<CompanyVM> data;
        int count;
        bool isLoading;

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            DatatableParams datatableParams = DataGridFilterCreator.Create(args, "Name");

            data = await CompanyService.ListAsync(_httpClient,datatableParams);
            count = data.Count() > 0 ? data[0].TotalRecords : 0;
            isLoading = false;
        }
    }
}
