using Microsoft.AspNetCore.Components;

namespace Web.UI.Pages.Aircraft
{
    public partial class Index
    {
        [Inject] NavigationManager? NavManager { get; set; }

        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    return base.OnAfterRenderAsync(firstRender);
        //}
        private void OpenDetails() 
        {
            NavManager.NavigateTo("/AircraftDetails?AircraftId=N0ZseU1hbmFnZXI=");

        }
    }
}
