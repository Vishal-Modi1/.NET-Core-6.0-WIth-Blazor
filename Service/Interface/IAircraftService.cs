﻿using DataModels.VM.Aircraft;
using DataModels.VM.Common;


namespace Service.Interface
{
    public interface IAircraftService
    {
        CurrentResponse Create(AircraftVM airCraftVM);
        CurrentResponse List(AircraftDatatableParams datatableParams);
        CurrentResponse Edit(AircraftVM airCraftVM);
        CurrentResponse GetDetails(int id, int companyId);
        CurrentResponse Delete(int id);
        CurrentResponse UpdateImageName(int id, string imageName);
        CurrentResponse IsAirCraftExist(int id, string tailNo);
        CurrentResponse GetFiltersValue(int companyId);

    }
}
