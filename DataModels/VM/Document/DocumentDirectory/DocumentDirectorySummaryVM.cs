namespace DataModels.VM.Document.DocumentDirectory
{
    public class DocumentDirectorySummaryVM
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int CompanyId { get; set; }

        public int TotalDocuments { get; set; }

        public int SortNo { get; set; }
    }
}
