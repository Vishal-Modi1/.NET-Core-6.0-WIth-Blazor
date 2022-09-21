using Microsoft.AspNetCore.Components;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Radzen;
using DataModels.Enums;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using Newtonsoft.Json;
using AMK = FSM.Blazor.Pages.AircraftMake;
using AMD = FSM.Blazor.Pages.AircraftModel;
using FSM.Blazor.Data.AircraftMake;
using Microsoft.AspNetCore.Components.Authorization;
using FSM.Blazor.Utilities;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class Create
    {
        [Parameter] public AircraftVM AircraftData { get; set; }

        [CascadingParameter] protected Task<AuthenticationState> AuthStat { get; set; }

        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        private CurrentUserPermissionManager _currentUserPermissionManager;

        public RadzenSteps steps;
        public RadzenTemplateForm<AircraftVM> form;

        public List<DropDownValues> YearDropDown { get; set; }
        public List<DropDownValues> NoofEnginesDropDown { get; set; }
        public List<DropDownValues> NoofPropellersDropDown { get; set; }

        public int CompanyId, Year, MakeId, ModelId, CategoryId, ClassId, FlightSimulatorId, NoofEnginesId, NoofPropellersId;
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isDisplayClassDropDown, isDisplayFlightSimulatorDropDown, isDisplayNoofEnginesDropDown,
            isDisplayEnginesHavePropellers, isDisplayEnginesareTurbines, isDisplayNoofPropellersDropDown, isBusySaveButton;

        bool isAircraftImageChanged, isDisplayMakePopup, isDisplayModelPopup;

        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            YearDropDown = new List<DropDownValues>();
            NoofEnginesDropDown = new List<DropDownValues>();
            NoofPropellersDropDown = new List<DropDownValues>();

            isAircraftImageChanged = false;
            AircraftData.IsEquipmentTimesListChanged = false;

            for (int year = 1; year <= 5; year++)
            {
                NoofEnginesDropDown.Add(new DropDownValues() { Id = year, Name = year.ToString() });
            }

            for (int year = 0; year <= 5; year++)
            {
                NoofPropellersDropDown.Add(new DropDownValues() { Id = year, Name = year.ToString() });
            }

            for (int year = 1945; year <= DateTime.Now.Year; year++)
            {
                YearDropDown.Add(new DropDownValues() { Id = year, Name = year.ToString() });
            }

            NoofEnginesId = 1;
            AircraftData.TrackOilandFuel = true;
            AircraftData.IsEnginesareTurbines = true;
            AircraftData.IsEngineshavePropellers = true;
            AircraftData.IsIdentifyMeterMismatch = true;

            CompanyId = AircraftData.CompanyId.GetValueOrDefault();
            Year = Convert.ToInt16(AircraftData.Year);
            MakeId = AircraftData.AircraftMakeId;
            ModelId = AircraftData.AircraftModelId;
            CategoryId = AircraftData.AircraftCategoryId;
            ClassId = AircraftData.AircraftClassId.GetValueOrDefault();
            FlightSimulatorId = AircraftData.FlightSimulatorClassId.GetValueOrDefault();
            NoofEnginesId = AircraftData.NoofEngines;
            NoofPropellersId = AircraftData.NoofPropellers.GetValueOrDefault();

            if (AircraftData.AircraftMakeList.Where(p => p.Id == int.MaxValue).Count() == 0)
            {
                AircraftData.AircraftMakeList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
                AircraftData.AircraftModelList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
            }

            OnCategoryDropDownValueChange(CategoryId, true);
            return base.OnInitializedAsync();
        }

        async Task Submit(AircraftVM airCraftData)
        {
            if (steps.SelectedIndex == 0)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await AircraftService.IsAircraftExistAsync(dependecyParams, airCraftData.Id, airCraftData.TailNo);
                bool isAircraftExist = ManageIsAircraftExistResponse(response, "");

                if (isAircraftExist)
                {
                    return;
                }

                airCraftData.CompanyId = CompanyId;
                airCraftData.Year = Year.ToString();
                airCraftData.AircraftMakeId = MakeId;
                airCraftData.AircraftModelId = ModelId;
                airCraftData.AircraftCategoryId = CategoryId;
                airCraftData.AircraftClassId = ClassId;
                airCraftData.FlightSimulatorClassId = FlightSimulatorId;
                airCraftData.NoofEngines = NoofEnginesId;
                airCraftData.NoofPropellers = NoofPropellersId;

                await OpenNextTab();
            }
            else
            {
                SetSaveButtonState(true);

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await AircraftService.SaveandUpdateAsync(dependecyParams, airCraftData);

                if (response != null)
                {
                    AircraftVM aircraft = JsonConvert.DeserializeObject<AircraftVM>(response.Data.ToString());

                    if (aircraft.Id > 0)
                    {
                        airCraftData.AircraftEquipmentTimesList.ForEach(z => z.AircraftId = aircraft.Id);
                        airCraftData.Id = aircraft.Id;

                        if (AircraftData.IsEquipmentTimesListChanged)
                        {
                            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                            response = await AircraftService.SaveandUpdateEquipmentTimeListAsync(dependecyParams, airCraftData);
                        }
                    }
                }

                SetSaveButtonState(false);

                if (!isAircraftImageChanged)
                {
                    ManageResponse(response, "Aircraft Details", true);
                }
                else
                {
                    SetSaveButtonState(true);

                    if (response != null && response.Status == System.Net.HttpStatusCode.OK && !string.IsNullOrWhiteSpace(airCraftData.ImagePath))
                    {
                        //data:image/gif;base64,
                        //this image is a single pixel (black)
                        byte[] bytes = Convert.FromBase64String(airCraftData.ImagePath.Substring(airCraftData.ImagePath.IndexOf(",") + 1));

                        ByteArrayContent data = new ByteArrayContent(bytes);

                        MultipartFormDataContent multiContent = new MultipartFormDataContent
                        {
                          { data, "0", "0" }
                        };

                        multiContent.Add(new StringContent(data.ToString()), "data");
                        multiContent.Add(new StringContent(airCraftData.Id.ToString()), "AircraftId");
                        multiContent.Add(new StringContent(airCraftData.CompanyId.ToString()), "CompanyId");

                        dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                        response = await AircraftService.UploadAircraftImageAsync(dependecyParams, multiContent);

                        ManageFileUploadResponse(response, "Aircraft Details", true);

                        SetSaveButtonState(false);
                    }
                }
            }
        }

        async Task OnModelValueChanged(object value)
        {
            if ((int)value == int.MaxValue)
            {

                isDisplayModelPopup = true;
            }
        }

        async Task OnMakeValueChanged(object value)
        {
            if ((int)value == int.MaxValue)
            {
                isDisplayMakePopup = true;
            }
        }

        void OnNoofPropellersDropDownValueChange()
        {
            AircraftData.IsEquipmentTimesListChanged = true;
        }

        void OnNoOfEngineDropDownValueChange()
        {
            AircraftData.IsEquipmentTimesListChanged = true;
        }

        void OnCategoryDropDownValueChange(object value, bool isManualTriggerdEvent = false)
        {
            if (!isManualTriggerdEvent)
            {
                AircraftData.IsEquipmentTimesListChanged = true;
            }

            if (Convert.ToInt16(value) == (int)AircraftCategory.Airplane)
            {
                isDisplayClassDropDown = true;
                isDisplayEnginesareTurbines = true;
                isDisplayNoofPropellersDropDown = true;
                isDisplayEnginesHavePropellers = true;

                if (AircraftData.Id == 0)
                {
                    ClassId = 0; NoofEnginesId = 0;
                }
            }
            else
            {
                isDisplayClassDropDown = false;
                isDisplayEnginesareTurbines = false;
                isDisplayNoofPropellersDropDown = false;
                isDisplayEnginesHavePropellers = false;

                if (Convert.ToInt16(value) == (int)AircraftCategory.Helicopter_Rotorcraft)
                {
                    isDisplayEnginesareTurbines = true;
                }
            }

            if (Convert.ToInt16(value) == (int)AircraftCategory.FlightSimulator)
            {
                isDisplayFlightSimulatorDropDown = true;
            }
            else
            {
                FlightSimulatorId = 0;
                isDisplayFlightSimulatorDropDown = false;
            }

            ManageNoofEngineDropdown();
        }

        void OnClassDropDownValueChange(object value)
        {
            AircraftData.IsEquipmentTimesListChanged = true;
            ManageNoofEngineDropdown();
        }

        private void ManageNoofEngineDropdown()
        {
            if ((ClassId == (int)AircraftClass.SingleEngineLand
                || ClassId == (int)AircraftClass.SingleEngineSea || ClassId == 0) && isDisplayClassDropDown)
            {
                isDisplayNoofEnginesDropDown = false;
                NoofEnginesId = 1;
            }
            else
            {
                isDisplayNoofEnginesDropDown = true;
            }
        }

        async Task OpenNextTab()
        {
            steps.SelectedIndex = 1;
            base.StateHasChanged();
        }

        void OpenPreviousTab()
        {
            steps.SelectedIndex = 0;
        }

        private bool ManageIsAircraftExistResponse(CurrentResponse response, string summary)
        {
            NotificationMessage message;
            bool isAircraftExist = false;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Data))
                {
                    isAircraftExist = true;
                    message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                    NotificationService.Notify(message);
                }
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }

            return isAircraftExist;
        }

        private async void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, response.Message);
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, summary, response.Message);
                NotificationService.Notify(message);
            }
        }

        private void ManageFileUploadResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong while uploading file!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(false);
                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, "Aircraft details added successfully.");
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went wrong while uploading file!", response.Message);
                NotificationService.Notify(message);
            }
        }

        private void SetSaveButtonState(bool isBusy)
        {
            isBusySaveButton = isBusy;
            StateHasChanged();
        }

        void OnFileChange()
        {
            isAircraftImageChanged = true;
        }

        public void CloseDialog(bool isCancelled)
        {
            CloseDialogCallBack.InvokeAsync(isCancelled);
        }

        public async Task CloseMakeDialogAsync(bool isCancelled)
        {
            if (!isCancelled)
            {

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                CurrentResponse response = await AircraftMakeService.ListDropdownValues(dependecyParams);
                AircraftData.AircraftMakeList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());

                AircraftData.AircraftMakeList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });

            }

            MakeId = 0;
            isDisplayMakePopup = false;
        }

        public async Task CloseModelDialogAsync(bool isCancelled)
        {
            if (!isCancelled)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await AircraftModelService.ListDropdownValues(dependecyParams);
                AircraftData.AircraftModelList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());

                AircraftData.AircraftModelList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
            }

            isDisplayModelPopup = false;
            ModelId = 0;
        }
    }
}
