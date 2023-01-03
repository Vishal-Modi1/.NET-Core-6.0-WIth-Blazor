using DataModels.Enums;
using Microsoft.AspNetCore.Components;

namespace Web.UI.Shared.Components.CustomPopup
{
    public partial class CustomPopupComponent
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
        [Parameter] public string? Width { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public bool IsModalLg { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter] public bool ShowHeaderCloseButton { get; set; } = true;
        
        /// <summary>
        /// 
        /// </summary>
        [Parameter] public RenderFragment? Footer { get; set; }
        [Parameter] public bool CloseOnOutsideClick { get; set; }
        [Parameter] public string HeaderCssClass { get; set; }
        [Parameter] public string HeaderTitleCssClass { get; set; }

        //[Parameter] public string BodyCssClass { get; set; }
        //[Parameter] public string FooterCssClass { get; set; }
        //[Parameter] public Category Type { get; set; }

        [Parameter] public EventCallback OnClose { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private Task Close()
        {
            return OnClose.InvokeAsync();
        }

        private Task OutsideClick()
        {
            if (CloseOnOutsideClick)
                return OnClose.InvokeAsync();

            return Task.CompletedTask;
        }

        public static string GetHeaderCssClass(OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Create: 
                    return "bg-primary-f text-white d-flex justify-content-center";
                case OperationType.Edit:
                    return "bg-primary-f text-white d-flex justify-content-center";
                case OperationType.Delete:
                    return "bg-danger-f text-white d-flex justify-content-center";
                default:
                    return "bg-primary-f text-white d-flex justify-content-center";
            }
        }

        //public static string GetHeaderTitleCssClass()
        //{
        //    return "text-white";
        //}

        //public enum Category
        //{
        //    Okay,
        //    SaveNot,
        //    DeleteNot
        //}

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
