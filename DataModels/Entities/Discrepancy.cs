namespace DataModels.Entities
{
    public class Discrepancy : CommonField
    {
        public long Id { get; set; }
        public long ReportedByUserId { get; set; }
        public long AircraftId { get; set; }
        public int CompanyId { get; set; }
        public byte DiscrepancyStatusId { get; set; }
        public string FileName { get; set; }
        public string FileDisplayName { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }
    }
}
