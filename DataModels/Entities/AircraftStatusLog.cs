namespace DataModels.Entities
{
    public class AircraftStatusLog : CommonField
    {
        public long Id { get; set; }
        public long AircraftId { get; set; }
        public byte AircraftStatusId { get; set; }
        public string Reason { get; set; }
    }
}
