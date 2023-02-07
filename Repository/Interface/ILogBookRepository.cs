using System.Collections.Generic;
using DataModels.Entities;
using DataModels.VM.Common;
using DataModels.VM.LogBook;

namespace Repository.Interface
{
    public interface ILogBookRepository : IBaseRepository<LogBook>
    {
        List<DropDownSmallValues> ListInstrumentApproachesDropdownValues();

        LogBookVM FindById(long id);

        LogBookVM Create(LogBookVM logBookVM);
        LogBookVM Edit(LogBookVM logBookVM);

        List<LogBookSummaryVM> LogBookSummaries(long userId, int companyId);

        #region flight photos
        List<LogBookFlightPhoto> ListFlightPhotosByLogBookId(long logbookId);

        void UpdateImagesName(long logbookId, List<LogBookFlightPhoto> logBookFlightPhotosList);

        void DeletePhoto(long id, long deletedBy);

        #endregion

        #region Crew Passengers

        List<DropDownSmallValues> ListPassengersRolesDropdownValues();

        List<DropDownLargeValues> ListPassengersDropdownValuesByCompanyId(int companyId);

        CrewPassenger SaveCrewPassenger(CrewPassenger crewPassenger);

        #endregion
    }
}
