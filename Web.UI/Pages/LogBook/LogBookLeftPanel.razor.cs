namespace Web.UI.Pages.LogBook
{
    public partial class LogBookLeftPanel
    {
        public bool isDayListVisible;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { }
            return base.OnAfterRenderAsync(firstRender);
        }

        private void AddEntry() 
        { 
        }
    }
}
