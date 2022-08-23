namespace Web.UI.Pages.Aircraft
{
    public partial class AircraftDetails
    {
        public int ActiveTabIndex { get; set; }

        private void NotDevelop() 
        { 

        }

        public bool IsTabSelectedFlag(int activeTab)
        {
            return ActiveTabIndex == activeTab;
        }
    }
}
