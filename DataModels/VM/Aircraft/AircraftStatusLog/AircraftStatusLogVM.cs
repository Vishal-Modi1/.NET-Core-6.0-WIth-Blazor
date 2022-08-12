using DataModels.Entities;

namespace DataModels.VM.Aircraft.AircraftStatusLog
{
    public class AircraftStatusLogVM : CommonField
    {
        public long Id { get; set; }
        public long AircraftId { get; set; }
        public byte AircraftStatusId { get; set; }
        public string AircraftStatus { get; set; }
        public string Reason { get; set; }
    }
}
