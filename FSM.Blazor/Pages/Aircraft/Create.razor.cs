using Microsoft.AspNetCore.Components;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Radzen;
using DataModels.Enums;
using Radzen.Blazor;
using FSM.Blazor.Extensions;
using DE = DataModels.Entities;
using Newtonsoft.Json;
using DataModels.Constants;
using AMK = FSM.Blazor.Pages.Aircraft.AircraftMake;
using AMD = FSM.Blazor.Pages.Aircraft.AircraftModel;
using FSM.Blazor.Data.AircraftMake;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class Create
    {
        [Parameter]
        public AirCraftVM AircraftData { get; set; }

        [Inject]
        IHttpClientFactory _httpClient { get; set; }

        public RadzenSteps steps;
        public RadzenTemplateForm<AirCraftVM> form;

        public List<DropDownValues> YearDropDown { get; set; }
        public List<DropDownValues> NoofEnginesDropDown { get; set; }
        public List<DropDownValues> NoofPropellersDropDown { get; set; }

        public int CompanyId, Year, MakeId, ModelId, CategoryId, ClassId, FlightSimulatorId, NoofEnginesId, NoofPropellersId;
        bool isPopup = Configuration.ConfigurationSettings.Instance.IsDiplsayValidationInPopupEffect;
        bool isDisplayClassDropDown, isDisplayFlightSimulatorDropDown, isDisplayNoofEnginesDropDown,
            isDisplayEnginesHavePropellers, isDisplayEnginesareTurbines, isDisplayNoofPropellersDropDown;

        protected override Task OnInitializedAsync()
        {
            YearDropDown = new List<DropDownValues>();
            NoofEnginesDropDown = new List<DropDownValues>();
            NoofPropellersDropDown = new List<DropDownValues>();

            for (int year = 1; year <= 5; year++)
            {
                NoofEnginesDropDown.Add(new DropDownValues() { Id = year, Name = year.ToString() });
            }

            for (int year = 0; year <= 5; year++)
            {
                NoofPropellersDropDown.Add(new DropDownValues() { Id = year, Name = year.ToString() });
            }

            for (int year = 1980; year <= DateTime.Now.Year; year++)
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

            AircraftData.AircraftMakeList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
            AircraftData.AircraftModelList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });

            OnCategoryDropDownValueChange(CategoryId);
            return base.OnInitializedAsync();
        }

        async Task Submit(AirCraftVM airCraftData)
        {
            if (steps.SelectedIndex == 0)
            {
                CurrentResponse response = await AircraftService.IsAircraftExistAsync(_httpClient, airCraftData.Id, airCraftData.TailNo);
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
                CurrentResponse response = await AircraftService.SaveandUpdateAsync(_httpClient, airCraftData);

                if (string.IsNullOrWhiteSpace(airCraftData.ImagePath))
                {
                    ManageResponse(response, "Aircraft Details", true);
                }
                else
                {
                    string uploadsPath = Path.Combine("", UploadDirectory.RootDirectory);
                    Directory.CreateDirectory(uploadsPath);

                    Directory.CreateDirectory(uploadsPath + "\\" + "uploads");

                    if (response != null && response.Status == System.Net.HttpStatusCode.OK)
                    {
                        //data:image/gif;base64,
                        //this image is a single pixel (black)
                        byte[] bytes = Convert.FromBase64String(airCraftData.ImagePath.Substring(airCraftData.ImagePath.IndexOf(",") + 1));


                        ByteArrayContent data = new ByteArrayContent(bytes);

                        airCraftData = JsonConvert.DeserializeObject<AirCraftVM>(response.Data);

                        MultipartFormDataContent multiContent = new MultipartFormDataContent
                    {
                        { data, "file", airCraftData.Id.ToString() }
                    };

                        response = await AircraftService.UploadAircraftImageAsync(_httpClient, multiContent);

                        ManageFileUploadResponse(response, "Aircraft Details", true);
                    }
                }
            }
        }

        async Task OnModelValueChanged(object value)
        {
            if ((int)value == int.MaxValue)
            {
                await DialogService.OpenAsync<AMD.Create>("Create",
                  null, new DialogOptions() { Width = "500px", Height = "380px" });

                CurrentResponse response = await AircraftModelService.ListDropdownValues(_httpClient);
                AircraftData.AircraftModelList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data);

                AircraftData.AircraftModelList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });

                ModelId = 0;
            }
        }

        async Task OnMakeValueChanged(object value)
        {
            if ((int)value == int.MaxValue)
            {
                await DialogService.OpenAsync<AMK.Create>("Create",
                  null, new DialogOptions() { Width = "500px", Height = "380px" });

                CurrentResponse response = await AircraftMakeService.ListDropdownValues(_httpClient);
                AircraftData.AircraftMakeList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data);
                
                AircraftData.AircraftMakeList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });

                MakeId = 0;
            }
        }

        void OnCategoryDropDownValueChange(object value)
        {
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
            ManageNoofEngineDropdown();
        }

        private void ManageNoofEngineDropdown()
        {
            if ((ClassId == (int)AircraftClass.SingleEngineLand
                || ClassId == (int)AircraftClass.SingleEngineSea || ClassId == 0) && isDisplayClassDropDown)
            {
                isDisplayNoofEnginesDropDown = false;
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

        private void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            NotificationMessage message;

            if (response == null)
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Something went Wrong!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (isCloseDialog)
                {
                    DialogService.Close(true);
                }

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
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft details added successfully. Something went wrong while uploading file!", "Please try again later.");
                NotificationService.Notify(message);
            }
            else if (response.Status == System.Net.HttpStatusCode.OK)
            {
                if (isCloseDialog)
                {
                    DialogService.Close(true);
                }

                message = new NotificationMessage().Build(NotificationSeverity.Success, summary, "Aircraft details added successfully.");
                NotificationService.Notify(message);
            }
            else
            {
                message = new NotificationMessage().Build(NotificationSeverity.Error, "Aircraft details added successfully. Something went wrong while uploading file!", response.Message);
                NotificationService.Notify(message);
            }
        }
    }
}
