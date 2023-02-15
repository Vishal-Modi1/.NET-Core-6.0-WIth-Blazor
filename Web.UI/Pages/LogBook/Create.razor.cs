using DataModels.VM.Common;
using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using Web.UI.Utilities;

namespace Web.UI.Pages.LogBook
{
    partial class Create
    {
        [Parameter] public LogBookVM logBookVM { get; set; }
        [Parameter] public EventCallback RefreshSummaries { get; set; }
        [Parameter] public List<DropDownLargeValues> AircraftsList { get; set; } 
        [Parameter] public List<DropDownSmallValues> RolesList { get; set; }
        [Parameter] public List<DropDownLargeValues> PassengersList { get; set; }
        [Parameter] public List<DropDownLargeValues> UsersList { get; set; }


        bool isLogBookEntryVisible = true;
        bool isTakeoffsLandingsVisible = true;
        bool isInstrumentVisible = true;
        bool isTrainingVisible = true;
        bool isRouteDistanceVisible = true;
        bool isStartEndVisible = true;
        bool isNightVisionGogglesVisible = true;
        bool isFlightPhotosVisible = true;
        bool isCommentsVisible = true;

        protected override Task OnInitializedAsync()
        {
            dependecyParams = DependecyParamsCreator.Create(HttpClient, "", "", AuthenticationStateProvider);
            return base.OnInitializedAsync();
        }

        private void AddEntry()
        {
            logBookVM = new LogBookVM();
        }


        public async Task Submit()
        {
            bool isPendingInsturmentArea = false;
            bool isPendingCrewPassengerArea = false;

            if (logBookVM.LogBookInstrumentVM.LogBookInstrumentApproachesList.
                Any(p=>p.InstrumentApproachId == 0 || string.IsNullOrWhiteSpace(p.Airport))
                )
            {
                isPendingInsturmentArea=true;
            }

            if (logBookVM.LogBookCrewPassengersList.Any(p=>p.RoleId == 0))
            {
                isPendingCrewPassengerArea = true;
            }

            string message = "";

            if(isPendingInsturmentArea)
            {
                message = "Airport and Approach is required fiel in intrument approach.";
            }

            if (isPendingCrewPassengerArea)
            {
                message += " Passenger and role is required.";
            }

            if (!string.IsNullOrWhiteSpace(message))
            {
                globalMembers.UINotification.DisplayCustomErrorNotification(globalMembers.UINotification.Instance, message);
                return;
            }

            isBusySubmitButton = true;
            CurrentResponse response = await LogBookService.SaveandUpdateAsync(dependecyParams, logBookVM);

            if (response.Status == HttpStatusCode.OK)
            {
                List<LogBookFlightPhotoVM> existingPhotosDetails = logBookVM.LogBookFlightPhotosList;
                logBookVM = JsonConvert.DeserializeObject<LogBookVM>(response.Data.ToString());
                await UploadImages(response, existingPhotosDetails);
            }
            else
            {
                globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
                isBusySubmitButton = false;
            }
        }

        public async Task UploadImages(CurrentResponse response, List<LogBookFlightPhotoVM> eixstingPhotosDetails)
        {
            if (logBookVM.LogBookFlightPhotosList.Any())
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                int i = 0;

                foreach (LogBookFlightPhotoVM logBookFlightPhotoVM in logBookVM.LogBookFlightPhotosList)
                {
                    i++;

                    LogBookFlightPhotoVM eixstingPhotoDetails = eixstingPhotosDetails.Where(p => p.DisplayName == logBookFlightPhotoVM.DisplayName).First();

                    if(eixstingPhotoDetails.ImageData == null)
                    {
                        continue;
                    }

                    multiContent.Add(new StringContent(logBookFlightPhotoVM.Id.ToString()), "Id");
                    multiContent.Add(new StringContent(logBookFlightPhotoVM.DisplayName), "DisplayName");

                    var imageContent = new ByteArrayContent(eixstingPhotoDetails.ImageData);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    multiContent.Add(imageContent, i.ToString(), logBookFlightPhotoVM.DisplayName);
                }

                multiContent.Add(new StringContent(logBookVM.Id.ToString()), "LogBookId");
                response = await LogBookService.UploadFlightPhotosAsync(dependecyParams, multiContent);
            }

            globalMembers.UINotification.DisplayNotification(globalMembers.UINotification.Instance, response);
            
            isBusySubmitButton = false;
            RefreshSummariesInfo();
        }

        public void RefreshSummariesInfo()
        {
            RefreshSummaries.InvokeAsync();
        }

        #region panels 

        void ToggleVisibility_LogBookEntry()
        {
            isLogBookEntryVisible = !isLogBookEntryVisible;
        }

        void ToggleVisibility_TakeoffsLandings()
        {
            isTakeoffsLandingsVisible = !isTakeoffsLandingsVisible;
        }

        void ToggleVisibility_Instrument()
        {
            isInstrumentVisible = !isInstrumentVisible;
        }

        void ToggleVisibility_Training()
        {
            isTrainingVisible = !isTrainingVisible;
        }
        void ToggleVisibility_RouteDistance()
        {
            isRouteDistanceVisible = !isRouteDistanceVisible;
        }

        void ToggleVisibility_StartEnd()
        {
            isStartEndVisible = !isStartEndVisible;
        }

        void ToggleVisibility_NightVisionGoggles()
        {
            isNightVisionGogglesVisible = !isNightVisionGogglesVisible;
        }

        void ToggleVisibility_FlightPhotos()
        {
            isFlightPhotosVisible = !isFlightPhotosVisible;
        }

        void ToggleVisibility_Comments()
        {
            isCommentsVisible = !isCommentsVisible;
        }

        #endregion
    }
}
