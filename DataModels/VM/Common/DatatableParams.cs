namespace DataModels.VM.Common
{
    public class DatatableParams
    {
        public string SearchText { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string SortOrderColumn { get; set; }

        public string OrderType { get; set; }

        public int CompanyId { get; set; }

        public Enums.UserRole UserRole { get; set; }

        public bool IsFromMyProfile { get; set; }
    }
}
