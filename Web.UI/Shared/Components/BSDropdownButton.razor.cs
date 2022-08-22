using Microsoft.AspNetCore.Components;

namespace Web.UI.Shared.Components
{
    public partial class BSDropdownButton
    {
        [Parameter]
        public List<Models.Shared.BSDropdownItem> BSDropdownItemList { get; set; }
        
        [Parameter]
        public string StyleClass { get; set; }

        protected bool ShowMenuItem = false;

        private void ToggleShowMenu()
        {
            ShowMenuItem = !ShowMenuItem;
        }
    }
}
