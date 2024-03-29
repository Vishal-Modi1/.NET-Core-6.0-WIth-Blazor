﻿using DataModels.VM.Aircraft;
using DataModels.Entities;
using Microsoft.AspNetCore.Components;
using DataModels.VM.AircraftEquipment;

namespace Web.UI.Pages.Aircraft
{
    public partial class AircraftEquipmentTimes
    {
        [Parameter]
        public AircraftVM aircraftData { get; set; }
        int engineIndex = 0, propellerIndex = 0;

        protected override Task OnInitializedAsync()
        {
            if (aircraftData.Id == 0 || aircraftData.IsEquipmentTimesListChanged)
            {
                aircraftData.AircraftEquipmentTimesList = new List<AircraftEquipmentTimeCreateVM>();

                aircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTimeCreateVM()
                {
                    EquipmentName = ("Hobbs Total")
                });

                for (int engineNo = 1; engineNo <= aircraftData.NoofEngines; engineNo++)
                {
                    aircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTimeCreateVM()
                    {
                        EquipmentName = ("Engine " + engineNo)
                    });
                }

                int propeller = 0;
                for (int proppellerNo = aircraftData.NoofEngines + 1; proppellerNo < (aircraftData.NoofPropellers + aircraftData.NoofEngines + 1); proppellerNo++)
                {
                    propeller++;
                    aircraftData.AircraftEquipmentTimesList.Add(new AircraftEquipmentTimeCreateVM()
                    {
                        EquipmentName = ("Propeller " + propeller)
                    });
                }
            }

            return base.OnInitializedAsync();
        }

        async void ManageHoursChange(int engineIndex, decimal value)
        {
            aircraftData.AircraftEquipmentTimesList[engineIndex].Hours = value;
            aircraftData.IsEquipmentTimesListChanged = true;
        }
    }
}
