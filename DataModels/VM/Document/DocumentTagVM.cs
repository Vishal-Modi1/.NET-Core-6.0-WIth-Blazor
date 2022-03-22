namespace DataModels.VM.Document
{
    public class DocumentTagVM
    {
        public int Id { get; set; }

        public string TagName { get; set; }

        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
    }
}
