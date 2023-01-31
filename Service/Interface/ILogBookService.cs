using DataModels.VM.Common;
using DataModels.VM.LogBook;

namespace Service.Interface
{
    public interface  ILogBookService
    {
        CurrentResponse ListInstrumentApproachesDropdownValues();

        CurrentResponse FindById(long id);

        CurrentResponse Create(LogBookVM logBook);
    }
}
