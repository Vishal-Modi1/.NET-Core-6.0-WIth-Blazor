using DataModels.VM.Common;

namespace DataModels.VM.Document
{
    public class DocumentDatatableParams : DatatableParams
    {
        public int ModuleId { get; set; }

        public long UserId { get; set; }

        public Enums.UserRole UserRole { get; set; }
    }
}
