using Microsoft.AspNetCore.Components;
using Web.UI.Data.Company.Settings;
using DataModels.VM.Company.Settings;
using Web.UI.Utilities;
using DataModels.VM.Common;
using Newtonsoft.Json;
using System.Globalization;

namespace Web.UI.Pages.Company.DetailsView.Settings.DateFormat
{
    partial class Index
    {
        [Parameter] public int CompanyId { get; set; }
        [Parameter] public int CompanyIdParam { get; set; }
        CompanyDateFormatVM companyDateFormat;

        string exampleDate = DateTime.Now.ToString();

        protected override async Task OnInitializedAsync()
        {
            companyDateFormat = new CompanyDateFormatVM();
            ChangeLoaderVisibilityAction(true);

            await LoadData();

            ChangeLoaderVisibilityAction(false);
        }

        public async Task LoadData()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            companyDateFormat = await CompanyDateFormatService.GetDefault(dependecyParams);
            companyDateFormat.DateFormatsList = await CompanyDateFormatService.ListDropDownValues(dependecyParams);
            companyDateFormat.CompanyId = CompanyIdParam;
            SetDateValue();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await CompanyDateFormatService.SetDefault(dependecyParams, companyDateFormat);

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                var dateList = companyDateFormat.DateFormatsList;
                companyDateFormat = JsonConvert.DeserializeObject<CompanyDateFormatVM>(response.Data.ToString());
                companyDateFormat.DateFormatsList = dateList;
            }

            isBusySubmitButton = false;
        }

        void OnDateFormatValueChanged(Int16 dateValue)
        {
            companyDateFormat.DateFormatId = dateValue;
            SetDateValue();
        }

        void SetDateValue()
        {
            if(companyDateFormat.DateFormatId > 0)
            {
                var exampleDateFormat = companyDateFormat.DateFormatsList.Where(p=>p.Id == companyDateFormat.DateFormatId).First();
                exampleDate = DateTime.Now.ToString(exampleDateFormat.Name, CultureInfo.InvariantCulture);
            }
        }
    }
}
