using Microsoft.AspNetCore.Components;
using DataModels.VM.Aircraft;
using DataModels.VM.Common;
using DataModels.Enums;
using Newtonsoft.Json;
using Web.UI.Data.AircraftMake;
using Web.UI.Utilities;
using Telerik.Blazor.Components;
using Microsoft.AspNetCore.Components.Forms;
using DataModels.Constants;

namespace Web.UI.Pages.Aircraft
{
    public partial class Create
    {
        [Parameter] public AircraftVM aircraftData { get; set; }
        [Parameter] public EventCallback<bool> CloseDialogCallBack { get; set; }

        public TelerikTabStrip steps;
        public List<DropDownValues> YearDropDown { get; set; }
        public List<DropDownValues> NoofEnginesDropDown { get; set; }
        public List<DropDownValues> NoofPropellersDropDown { get; set; }

        bool isDisplayClassDropDown, isDisplayFlightSimulatorDropDown, isDisplayNoofEnginesDropDown,
            isDisplayEnginesHavePropellers, isDisplayEnginesareTurbines, isDisplayNoofPropellersDropDown;

        bool isAircraftImageChanged, isDisplayMakePopup, isDisplayModelPopup;
        byte[] aircraftImageBytes;
        public bool isAllowToLock;
        protected override Task OnInitializedAsync()
        {
            _currentUserPermissionManager = CurrentUserPermissionManager.GetInstance(MemoryCache);
            
            isAircraftImageChanged = false;
            aircraftData.IsEquipmentTimesListChanged = false;

            SetDropdownValues();

            long userId = Convert.ToInt64(_currentUserPermissionManager.GetClaimValue(AuthStat, CustomClaimTypes.UserId).Result);
            bool isOwner = userId == aircraftData.OwnerId;

            isAllowToLock = globalMembers.IsAdmin || globalMembers.IsSuperAdmin || isOwner || aircraftData.Id == 0;

            OnCategoryDropDownValueChange(aircraftData.AircraftCategoryId, true);
            return base.OnInitializedAsync();
        }

        private void SetDropdownValues()
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

            for (int year = 1945; year <= DateTime.Now.Year; year++)
            {
                YearDropDown.Add(new DropDownValues() { Id = year, Name = year.ToString() });
            }

            if (aircraftData.NoofEngines == 0)
            {
                aircraftData.NoofEngines = 1;
            }

            if (aircraftData.AircraftMakeList.Where(p => p.Id == int.MaxValue).Count() == 0)
            {
                aircraftData.AircraftMakeList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
                aircraftData.AircraftModelList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
            }

            if (aircraftData.AircraftStatusId == 0)
            {
                aircraftData.AircraftStatusId = 1;
            }
        }

        private async Task OnInputFileChangeAsync(InputFileChangeEventArgs e)
        {
            try
            {
                string fileType = Path.GetExtension(e.File.Name);
                List<string> supportedImagesFormatsList = supportedImagesFormat.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!supportedImagesFormatsList.Contains(fileType))
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, "File type is not supported");
                    return;
                }

                if (e.File.Size > maxProfileImageUploadSize)
                {
                    globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, $"File size exceeds maximum limit { maxProfileImageUploadSize / (1024 * 1024) } MB.");
                    return;
                }

                MemoryStream ms = new MemoryStream();
                await e.File.OpenReadStream(maxProfileImageUploadSize).CopyToAsync(ms);
                aircraftImageBytes = ms.ToArray();

                isAircraftImageChanged = true;
            }
            catch (Exception ex)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, ex.ToString());
            }
        }

        async Task Submit()
        {
            if (steps.ActiveTabIndex == 0)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await AircraftService.IsAircraftExistAsync(dependecyParams, aircraftData.Id, aircraftData.TailNo);
                bool isAircraftExist = ManageIsAircraftExistResponse(response, "");

                if (isAircraftExist)
                {
                    return;
                }

                OpenNextTab();
            }
            else
            {
                isBusySubmitButton = true;

                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await AircraftService.SaveandUpdateAsync(dependecyParams, aircraftData);

                if (response != null)
                {
                    AircraftVM aircraft = JsonConvert.DeserializeObject<AircraftVM>(response.Data.ToString());

                    if (aircraft.Id > 0)
                    {
                        aircraftData.AircraftEquipmentTimesList.ForEach(z => z.AircraftId = aircraft.Id);
                        aircraftData.Id = aircraft.Id;

                        if (aircraftData.IsEquipmentTimesListChanged)
                        {
                            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                            response = await AircraftService.SaveandUpdateEquipmentTimeListAsync(dependecyParams, aircraftData);
                        }
                    }
                }

                isBusySubmitButton = false;

                if (!isAircraftImageChanged)
                {
                    response.Message = "Aircraft Added Successfully";
                    ManageResponse(response, "Aircraft Details", true);
                }
                else
                {
                    isBusySubmitButton = true;

                    if (response.Status == System.Net.HttpStatusCode.OK && aircraftImageBytes != null)
                    {
                        //data:image/gif;base64,
                        //this image is a single pixel (black)
                        //byte[] bytes = Convert.FromBase64String(aircraftData.ImagePath.Substring(aircraftData.ImagePath.IndexOf(",") + 1));

                        ByteArrayContent data = new ByteArrayContent(aircraftImageBytes);

                        MultipartFormDataContent multiContent = new MultipartFormDataContent
                        {
                          { data, "0", "0" }
                        };

                        multiContent.Add(new StringContent(data.ToString()), "data");
                        multiContent.Add(new StringContent(aircraftData.Id.ToString()), "AircraftId");
                        multiContent.Add(new StringContent(aircraftData.CompanyId.ToString()), "CompanyId");

                        dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                        response = await AircraftService.UploadAircraftImageAsync(dependecyParams, multiContent);

                        ManageFileUploadResponse(response, "Aircraft Details", true);

                        isBusySubmitButton = false;
                    }
                }
            }
        }

        async Task OnModelValueChanged(int value)
        {
            if (value == int.MaxValue)
            {
                isDisplayModelPopup = true;
            }

            aircraftData.AircraftModelId = value;
        }

        async Task OnMakeValueChanged(int value)
        {
            if (value == int.MaxValue)
            {
                isDisplayMakePopup = true;
            }

            aircraftData.AircraftMakeId = value;
        }

        void OnNoofPropellersDropDownValueChange(int? value)
        {
            if (aircraftData.NoofPropellers != value)
            {
                aircraftData.NoofPropellers = value.GetValueOrDefault();
                aircraftData.IsEquipmentTimesListChanged = true;
            }
        }

        void OnNoOfEngineDropDownValueChange(int value)
        {
            if (aircraftData.NoofEngines != value)
            {
                aircraftData.NoofEngines = value;
                aircraftData.IsEquipmentTimesListChanged = true;
            }
        }

        void OnCategoryDropDownValueChange(int value, bool isManualTriggerdEvent = false)
        {
            if (aircraftData.AircraftCategoryId == value)
            {
                return;
            }

            aircraftData.AircraftCategoryId = value;

            if (!isManualTriggerdEvent)
            {
                aircraftData.IsEquipmentTimesListChanged = true;
            }

            if (Convert.ToInt16(value) == (int)AircraftCategory.Airplane)
            {
                isDisplayClassDropDown = true;
                isDisplayEnginesareTurbines = true;
                isDisplayNoofPropellersDropDown = true;
                isDisplayEnginesHavePropellers = true;

                if (aircraftData.Id == 0)
                {
                    aircraftData.AircraftClassId = 0; aircraftData.NoofEngines = 0;
                }
            }
            else
            {
                isDisplayClassDropDown = false;
                isDisplayEnginesareTurbines = false;
                isDisplayNoofPropellersDropDown = false;
                isDisplayEnginesHavePropellers = false;

                if (value == (int)AircraftCategory.Helicopter_Rotorcraft)
                {
                    isDisplayEnginesareTurbines = true;
                }
            }

            if (value == (int)AircraftCategory.FlightSimulator)
            {
                isDisplayFlightSimulatorDropDown = true;
            }
            else
            {
                aircraftData.FlightSimulatorClassId = 0;
                isDisplayFlightSimulatorDropDown = false;
            }

            ManageNoofEngineDropdown();
        }

        void OnClassDropDownValueChange(int? value)
        {
            if (aircraftData.AircraftClassId != value)
            {
                aircraftData.AircraftClassId = value.GetValueOrDefault();
                aircraftData.IsEquipmentTimesListChanged = true;
                ManageNoofEngineDropdown();
            }
        }

        private void ManageNoofEngineDropdown()
        {
            if ((aircraftData.AircraftClassId == (int)AircraftClass.SingleEngineLand
                || aircraftData.AircraftClassId == (int)AircraftClass.SingleEngineSea || aircraftData.AircraftClassId == 0) && isDisplayClassDropDown)
            {
                isDisplayNoofEnginesDropDown = false;
                aircraftData.NoofEngines = 1;
            }
            else
            {
                isDisplayNoofEnginesDropDown = true;
            }
        }

        void OpenNextTab()
        {
            steps.ActiveTabIndex = 1;
        }

        void OpenPreviousTab()
        {
            steps.ActiveTabIndex = 0;
        }

        private bool ManageIsAircraftExistResponse(CurrentResponse response, string summary)
        {
            bool isAircraftExist = false;
            
            if ((bool)response.Data == true)
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, response.Message);
                isAircraftExist = true;
            }

            return isAircraftExist;
        }

        private async void ManageResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        private void ManageFileUploadResponse(CurrentResponse response, string summary, bool isCloseDialog)
        {
            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);

            if (response.Status == System.Net.HttpStatusCode.OK)
            {
                CloseDialog(true);
            }
        }

        void OnFileChange()
        {
            isAircraftImageChanged = true;
        }

        public void CloseDialog(bool refreshData)
        {
            CloseDialogCallBack.InvokeAsync(refreshData);
        }

        public async Task CloseMakeDialogAsync(bool reloadData)
        {
            if (reloadData)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);

                CurrentResponse response = await AircraftMakeService.ListDropdownValues(dependecyParams);
                aircraftData.AircraftMakeList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());
                aircraftData.AircraftMakeList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
            }

            aircraftData.AircraftMakeId = 0;
            isDisplayMakePopup = false;
        }

        public async Task CloseModelDialogAsync(bool reloadData)
        {
            if (reloadData)
            {
                DependecyParams dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
                CurrentResponse response = await AircraftModelService.ListDropdownValues(dependecyParams);
                aircraftData.AircraftModelList = JsonConvert.DeserializeObject<List<DropDownValues>>(response.Data.ToString());

                aircraftData.AircraftModelList.Add(new DropDownValues() { Id = int.MaxValue, Name = "Add New ++" });
            }

            isDisplayModelPopup = false;
            aircraftData.AircraftModelId = 0;
        }
    }
}
