namespace DataModels.Entities
{
    public class DiscrepancyFile : CommonField
    {
        public long Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public long DiscrepancyId { get; set; }
    }
}
