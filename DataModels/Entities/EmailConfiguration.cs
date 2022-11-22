namespace DataModels.Entities
{
    public class EmailConfiguration
    {
        public long Id { get; set; }

        public byte EmailTypeId { get; set; }

        public string Email { get; set; }

        public int CompanyId { get; set; }
    }
}
