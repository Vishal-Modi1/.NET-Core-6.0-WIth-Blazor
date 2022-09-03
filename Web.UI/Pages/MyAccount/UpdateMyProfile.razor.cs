using Microsoft.AspNetCore.Components;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using DataModels.Enums;
using Newtonsoft.Json;
using Web.UI.Data.AircraftMake;
using Web.UI.Utilities;
using Telerik.Blazor.Components;
using DataModels.VM.User;

namespace Web.UI.Pages.MyAccount
{
    public partial class UpdateMyProfile
    { 
        [Parameter] public UserVM userVM { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }



        #region Gender Module
        TelerikRadioGroup<GenderModel, int?> RadioGroupRef { get; set; }

        int ChosenGender { get; set; }

        List<GenderModel> GenderOptions { get; set; } = new List<GenderModel>
        {
            new GenderModel { GenderId = 1,GenderText = "Female" },
            new GenderModel { GenderId = 2,GenderText = "Male" },
        };

        public class GenderModel
        {
            public int GenderId { get; set; }
            public string GenderText { get; set; }
        }
        #endregion

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            //ADD CODE HERE
            return base.OnInitializedAsync();
        }

        async Task Submit()
        {
                isBusySubmitButton = true;
        }

        public void CloseDialog(bool refreshData)
        {
            CloseDialogCallBack.InvokeAsync(refreshData);
        }
    }
}
