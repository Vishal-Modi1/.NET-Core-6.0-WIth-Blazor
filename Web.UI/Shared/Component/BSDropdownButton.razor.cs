using Microsoft.AspNetCore.Components;

namespace Web.UI.Shared.Component
{
    public partial class BSDropdownButton
    {
        [Parameter]
        public List<Web.UI.Models.Shared.BSDropdownItem> BSDropdownItemList { get; set; }
        
        [Parameter]
        public string StyleClass { get; set; }

        protected bool ShowMenuItem = false;
        private void ToggleShowMenu()
        {
            ShowMenuItem = !ShowMenuItem;
        }
    }
}
