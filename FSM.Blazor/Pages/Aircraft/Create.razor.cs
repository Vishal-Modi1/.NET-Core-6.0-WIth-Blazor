using Microsoft.AspNetCore.Components;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using Radzen;
using DataModels.Enums;
using Radzen.Blazor;
using Microsoft.AspNetCore.Components.Forms;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class Create
    {
        [Parameter]
        public AirCraftVM airCraftData { get; set; }

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
            airCraftData.TrackOilandFuel = true;
            airCraftData.IsEnginesareTurbines = true;
            airCraftData.IsEngineshavePropellers = true;
            airCraftData.IsIdentifyMeterMismatch = true;

            return base.OnInitializedAsync();
        }

        async Task Submit(AirCraftVM airCraftData)
        {
            airCraftData.CompanyId = CompanyId;
            airCraftData.Year = Year.ToString();
            airCraftData.AircraftMakeId = MakeId;
            airCraftData.AircraftModelId = ModelId;
            airCraftData.AircraftCategoryId = CategoryId;
            airCraftData.AircraftClassId = ClassId;
            airCraftData.FlightSimulatorClassId = FlightSimulatorId;
            airCraftData.NoofEngines = NoofEnginesId;
            airCraftData.NoofPropellers = NoofPropellersId;

            OpenNextTab();
        }

        void OnAircraftImageChange(string value, string name)
        {
        }

        void OnAircraftImageError(UploadErrorEventArgs args, string name)
        {

        }

        void OnCategoryDropDownValueChange(object value)
        {
            string selectedCategory = airCraftData.AircraftCategoryList.Where(p => p.Id == (int)value).Select(p => p.Name).FirstOrDefault();

            if (Convert.ToInt16(value) == (int)AircraftCategory.Airplane)
            {
                isDisplayClassDropDown = true;
                isDisplayEnginesareTurbines = true;
                isDisplayNoofPropellersDropDown = true;
                isDisplayEnginesHavePropellers = true;

                ClassId = 0; NoofEnginesId = 0;

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
            var valid = true;

            valid = form.IsValid;

            steps.SelectedIndex = 1;
        }

        void OpenPreviousTab()
        {
            steps.SelectedIndex = 0;
        }
    }
}
