using DataModels.VM.Aircraft;
using DataModels.Entities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.AircraftEquipment;

namespace FSM.Blazor.Pages.Aircraft
{
    public partial class AircraftEquipmentTimes
    {
        [Parameter]
        public AircraftVM AircraftData { get; set; }


        int engineIndex = 0, propellerIndex = 0;

        protected override Task OnInitializedAsync()
        {
            if (AircraftData.Id == 0 || AircraftData.IsEquipmentTimesListChanged)
            {
                AircraftData.AircraftEquipmentTimesList = new List<AircraftEquipmentTimeCreateVM>();

                AircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTimeCreateVM()
                {
                    EquipmentName = ("Air Frame")
                });

                for (int engineNo = 1; engineNo <= AircraftData.NoofEngines; engineNo++)
                {
                    AircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTimeCreateVM()
                    {
                        EquipmentName = ("Engine " + engineNo)
                    });
                }

                int propeller = 0;
                for (int proppellerNo = AircraftData.NoofEngines + 1; proppellerNo < (AircraftData.NoofPropellers + AircraftData.NoofEngines + 1); proppellerNo++)
                {
                    propeller++;
                    AircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTimeCreateVM()
                    {
                        EquipmentName = ("Propeller " + propeller)
                    });
                }
            }

            return base.OnInitializedAsync();
        }

        async void ManageHoursChange(int engineIndex, object value)
        {
            AircraftData.AircraftEquipmentTimesList[engineIndex].Hours = Convert.ToDecimal(value);

        }
    }
}
