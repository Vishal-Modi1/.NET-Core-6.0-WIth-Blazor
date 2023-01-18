using DataModels.Entities;
using DataModels.VM.Common;
using Microsoft.AspNetCore.Components;
using Web.UI.Utilities;
using System.Drawing;
using DataModels.VM.Scheduler;

namespace Web.UI.Pages.Scheduler
{
    partial class CreateCategory
    {
        [Parameter] public FlightCategoryVM flightCategory { get; set; }
        [Parameter] public List<DropDownValues> Companies { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }
        string originalColor = "";

        protected override void OnParametersSet()
        {
            if (globalMembers.IsSuperAdmin && !Companies.Any(p=> p.Id == int.MaxValue))
            {
                Companies.Insert(0, new DropDownValues() { Id = int.MaxValue, Name = "All Companies" });
                base.StateHasChanged();
            }

            base.OnParametersSet();
        }

        protected override Task OnParametersSetAsync()
        {
            originalColor = flightCategory.Color;
            return base.OnParametersSetAsync();
        }

        public async Task Submit()
        {
            isBusySubmitButton = true;

            if (originalColor != flightCategory.Color)
            {
                var data = flightCategory.Color.Substring(4, flightCategory.Color.Length - 5).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => Convert.ToInt32(p)).ToList();
                Color myColor = Color.FromArgb(data[0], data[1], data[2]);
                flightCategory.Color = "#" + myColor.R.ToString("X2") + myColor.G.ToString("X2") + myColor.B.ToString("X2");
            }

            if (flightCategory.CompanyId == int.MaxValue && globalMembers.IsSuperAdmin)
            {
                flightCategory.IsForAllCompanies = true;
            }

            DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            CurrentResponse response = await FlightCategoryService.SaveandUpdateAsync(dependecyParams, flightCategory);

            isBusySubmitButton = false;

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }

            else if (response.Status == System.Net.HttpStatusCode.Ambiguous)
            {
                return;
            }
            else
            {
                CloseDialog(false);
            }
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }
    }
}
