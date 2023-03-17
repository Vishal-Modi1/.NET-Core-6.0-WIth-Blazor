using DataModels.VM.Common;
using DataModels.VM.LogBook;
using DataModels.Entities;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ILogBookService
    {
        CurrentResponse ListInstrumentApproachesDropdownValues();

        CurrentResponse DeleteLogBookInstrumentApproach(long id, long deletedBy);

        CurrentResponse FindById(long id);

        CurrentResponse Create(LogBookVM logBook);

        CurrentResponse Edit(LogBookVM logBookVM);

        CurrentResponse GetFiltersValue(string role, int companyId);

        CurrentResponse List(LogBookDatatableParams datatableParams);

        CurrentResponse LogBookSummaries(long userId, int companyId, string role);

        CurrentResponse Delete(long id, long deletedBy);

        CurrentResponse ListArrivalAirportsDropDownValuesByCompanyId(int companyId);
        CurrentResponse ListDepartureAirportsDropDownValuesByCompanyId(int companyId);

        #region flight photos
        List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logBookId);

        CurrentResponse UpdateImagesName(long logBookId, List<LogBookFlightPhoto> logBookFlightPhotosList);

        CurrentResponse DeletePhoto(long id, long deletedBy);

        #endregion

        #region Crew Passengers

        CurrentResponse ListPassengersRolesDropdownValues();

        CurrentResponse ListPassengersDropdownValuesByCompanyId(int companyId);

        CurrentResponse CreateCrewPassenger(CrewPassengerVM crewPassengerVM);

        CurrentResponse DeleteLogBookCrewPassenger(long id, long deletedBy);



        #endregion
    }
}
