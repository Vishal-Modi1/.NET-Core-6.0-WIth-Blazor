using Microsoft.AspNetCore.Components;
namespace Web.UI.Shared.Components.CustomCollapseBar
{
    public partial class CustomCollapseBar
    {
        [Parameter]
        public bool IsFilterBarVisible { get; set; } = false;
         [Parameter]
        public bool IsExpanded { get; set; } = false;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        
        [Parameter]
        public RenderFragment? HeaderContent { get; set; }
        [Parameter]
        public RenderFragment? Header { get; set; }
        
        [Parameter]
        public string Class { get; set; } 
        
        [Parameter]
        public EventCallback OnPanelExpanded { get; set; }

        [Parameter]
        public EventCallback OnPanelCollapsed { get; set; }

        private void ToggleCollapseExpande() 
        {
            IsFilterBarVisible = !IsFilterBarVisible;
            if (IsFilterBarVisible)
            {
                OnPanelExpanded.InvokeAsync();
            }
            else {
                OnPanelCollapsed.InvokeAsync();
            }

        }
}
}
