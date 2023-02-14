using DataModels.VM.Common;

namespace DataModels.VM.LogBook
{
    public class LogBookDatatableParams : DatatableParams
    {
        public long UserId { get; set; }

        public long? AircraftId { get; set; }
    }
}
