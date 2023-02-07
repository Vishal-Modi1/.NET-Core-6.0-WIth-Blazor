using DataModels.VM.Common;
using DataModels.VM.LogBook;
using DataModels.Entities;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface ILogBookService
    {
        CurrentResponse ListInstrumentApproachesDropdownValues();

        CurrentResponse FindById(long id);

        CurrentResponse Create(LogBookVM logBook);

        CurrentResponse Edit(LogBookVM logBookVM);

        CurrentResponse LogBookSummaries(long userId, int companyId);

        #region flight photos
        List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logBookId);

        CurrentResponse UpdateImagesName(long logBookId, List<LogBookFlightPhoto> logBookFlightPhotosList);

        CurrentResponse DeletePhoto(long id, long deletedBy);

        #endregion

        #region Crew Passengers

        CurrentResponse ListPassengersRolesDropdownValues();

        CurrentResponse ListPassengersDropdownValuesByCompanyId(int companyId);

        CurrentResponse CreateCrewPassenger(CrewPassengerVM crewPassengerVM);

        #endregion
    }
}
