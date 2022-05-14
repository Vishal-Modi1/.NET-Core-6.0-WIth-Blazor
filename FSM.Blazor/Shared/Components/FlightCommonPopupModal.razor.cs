using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Shared.Components
{
    public partial class FlightCommonPopupModal
    {
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter] public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter] public RenderFragment? Body { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public RenderFragment? Footer { get; set; }
        [Parameter] public bool CloseOnOutsideClick { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        [Parameter] public Category Type { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private Task Cancel()
        {
            return OnClose.InvokeAsync(false);
        }
        private Task Ok()
        {
            return OnClose.InvokeAsync(true);
        }
        public enum Category
        {
            Okay,
            SaveNot,
            DeleteNot
        }

        //private string _title = "";
        //private string _description = "";
        //public bool IsOpen { get; set; }

        //[Parameter]
        //public bool CanClose { get; set; }

        //[Parameter]
        //public RenderFragment Body { get; set; }

        //[Parameter]
        //public string Title { get; set; }

        //[Parameter]
        //public string HeaderCssClass { get; set; }

        //[Parameter]
        //public string BodyCssClass { get; set; }

        //[Parameter]
        //public string FooterCssClass { get; set; }

        //[Parameter]
        //public string ModalSize { get; set; } = "modal-lg";

        //public void Open() => IsOpen = true;
        //public void Close() => IsOpen = false;
    }
}
