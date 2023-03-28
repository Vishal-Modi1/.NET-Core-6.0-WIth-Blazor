namespace DataModels.Entities
{
    public  class DocumentDirectory : CommonField
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; } = "";

        public int CompanyId { get; set; }
    }
}
