namespace Web.UI.Pages.LogBook
{
    partial class Index
    {
        int cureActiveTabIndex;

        void TabChangedHandler(int newIndex)
        {
            cureActiveTabIndex = newIndex;
        }
    }
}
