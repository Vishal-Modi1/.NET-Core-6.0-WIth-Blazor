using DataModels.VM.Aircraft;
using DataModels.Entities;
using Microsoft.AspNetCore.Components;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class AircraftEquipmentTimes
    {
        [Parameter]
        public AirCraftVM AircraftData { get; set; }


        int engineIndex = 0, propellerIndex = 0;

        protected override Task OnInitializedAsync()
        {

            if (AircraftData.Id == 0)
            {
                AircraftData.AircraftEquipmentTimesList = new List<AircraftEquipmentTime>();
            }

            AircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTime()
            {
                EquipmentName = ("Air Frame")
            });


            for (int engineNo = 1; engineNo <= AircraftData.NoofEngines; engineNo++)
            {
                AircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTime()
                {
                    EquipmentName = ("Engine " + engineNo)
                });
            }

            int propeller = 0;
            for (int proppellerNo = AircraftData.NoofEngines + 1; proppellerNo < (AircraftData.NoofPropellers + AircraftData.NoofEngines + 1); proppellerNo++)
            {
                propeller++;
                AircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTime()
                {
                    EquipmentName = ("Propeller " + propeller)
                });
            }

            return base.OnInitializedAsync();
        }

        async void ManageHoursChange(int engineIndex, object value)
        {
            AircraftData.AircraftEquipmentTimesList[engineIndex].Hours = Convert.ToInt16(value);

        }
    }
}
