using Microsoft.AspNetCore.Components;

namespace Web.UI.Shared.Components.CustomIcon
{
    public partial class CustomIcons
    {
        [Parameter]
        public string IconName { get; set; }
        [Parameter]
        public string PrimaryColor { get; set; } 
        [Parameter]
        public string SecondaryColor { get; set; }

        [Parameter]
        public bool IsTransparent { get; set; }

        string iconHeight = "15";
        string iconWidth = "15";

    }
}
