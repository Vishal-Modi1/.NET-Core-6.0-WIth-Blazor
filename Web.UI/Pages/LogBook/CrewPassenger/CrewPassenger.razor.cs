using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using DataModels.VM.Common;
using DataModels.Enums;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook.CrewPassenger
{
    partial class CrewPassenger
    {
        [Parameter] public List<LogBookCrewPassengerVM> LogBookCrewPassengersList { get; set; }
        [Parameter] public List<DropDownLargeValues> CrewPassengersList { get; set; }
        [Parameter] public List<DropDownSmallValues> RolesList { get; set; }
        [Parameter] public List<DropDownLargeValues> PassengersList { get; set; }
        [Parameter] public List<DropDownLargeValues> UsersList { get; set; }

        bool isCrewPassagnersVisible = true;

        LogBookCrewPassengerVM logBookCrewPassenger;
        public CrewPassengerVM crewPassengerVM { get; set; }

        protected override Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            return base.OnInitializedAsync();
        }

        async Task OpenDeleteDialog(LogBookCrewPassengerVM selectedPhoto)
        {
            isDisplayPopup = true;
            operationType = OperationType.Delete;
            popupTitle = "Delete Crew & Passenger";

            logBookCrewPassenger = selectedPhoto;
        }

        void SelectNewCrewPassenger()
        {
            LogBookCrewPassengersList.Add(new LogBookCrewPassengerVM());
        }

        void AddNewCrewPassenger()
        {
            isDisplayPopup = true;
            crewPassengerVM = new CrewPassengerVM();
            popupTitle = "Add new passenger";
        }

        void ToggleVisibility_CrewPassagners()
        {
            isCrewPassagnersVisible = !isCrewPassagnersVisible;
        }

        async Task DeleteAsync()
        {


            if (logBookCrewPassenger.Id > 0)
            {
                isBusyDeleteButton = true;
                CurrentResponse response = await LogBookService.DeleteLogBookCrewPassengerAsync(dependecyParams, logBookCrewPassenger.Id);

                isBusyDeleteButton = false;

                if (response.Status == System.Net.HttpStatusCode.OK)
                {
                    isDisplayPopup = false;
                    LogBookCrewPassengersList.Remove(logBookCrewPassenger);
                    globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
                }
            }
            else
            {
                LogBookCrewPassengersList.Remove(logBookCrewPassenger);
                globalMembers.UINotification.DisplaySuccessNotification(globalMembers.UINotification.Instance, "Passenger deleted successfully");
            }

        }
    }
}
