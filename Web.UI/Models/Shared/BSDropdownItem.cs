using Microsoft.AspNetCore.Components;

namespace Web.UI.Models.Shared
{
    public class BSDropdownItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string DisplayValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ActualValue { get; set; }

        public EventCallback OnClickFunction { get; set; }
        public string URL { get; set; }
    }
}
