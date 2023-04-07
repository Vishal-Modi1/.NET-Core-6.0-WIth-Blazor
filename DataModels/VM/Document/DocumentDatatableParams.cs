using DataModels.VM.Common;

namespace DataModels.VM.Document
{
    public class DocumentDatatableParams : DatatableParams
    {
        public int ModuleId { get; set; }

        public long UserId { get; set; }

        public long? AircraftId { get; set; }

        public string DocumentType { get; set; }

        public string TagIds { get; set; }

        public bool IsIgnoreTagFilter { get; set; }

        public bool IncludeDocumentsWithoutTags { get; set; }
    }
}
