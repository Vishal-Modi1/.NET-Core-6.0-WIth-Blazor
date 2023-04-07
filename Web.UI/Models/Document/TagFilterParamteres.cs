namespace Web.UI.Models.Document
{
    public class TagFilterParamteres
    {
        public string TagIds { get; set; }

        public bool IsIgnoreTagFilter { get; set; } = true;

        public bool IncludeDocumentsWithoutTags { get; set; } = true;
    }
}
