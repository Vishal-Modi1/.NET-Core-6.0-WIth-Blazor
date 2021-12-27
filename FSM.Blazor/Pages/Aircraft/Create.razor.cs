using Microsoft.AspNetCore.Components;
using DataModels.VM.Aircraft;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class Create
    {
        [Parameter]
        public AirCraftVM airCraftData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

    }
}
