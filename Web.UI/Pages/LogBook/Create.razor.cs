using DataModels.VM.LogBook;
using Microsoft.AspNetCore.Components;
using System;

namespace Web.UI.Pages.LogBook
{
    partial class Create
    {
        [Parameter] public LogBookVM logBookVM { get; set; }

        bool isLogBookEntryVisible = true;
        bool isTakeoffsLandingsVisible = true;
        bool isInstrumentVisible = true;
        bool isTrainingVisible = true;
        bool isRouteDistanceVisible = true;
        bool isStartEndVisible = true;
        bool isNightVisionGogglesVisible = true;
        bool isCrewPassagnersVisible = true;
        bool isFlightPhotosVisible = true;
        bool isCommentsVisible = true;
        void AddNewCrewPassenger()
        {
            logBookVM.LogBookCrewPassengersList.Add(new LogBookCrewPassengerVM());
        }

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

        void ToggleVisibility_CrewPassagners()
        {
            isCrewPassagnersVisible = !isCrewPassagnersVisible;
        }

        void ToggleVisibility_FlightPhotos()
        {
            isFlightPhotosVisible = !isFlightPhotosVisible;
        }

        void ToggleVisibility_Comments()
        {
            isCommentsVisible = !isCommentsVisible;
        }
    }
}
