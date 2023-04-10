namespace DataModels.VM.Common
{
    public class DatatableParams
    {
        public string SearchText { get; set; }

        public int Start { get; set; } = 1;

        public int Length { get; set; } = 5;

        public string SortOrderColumn { get; set; } = "CreatedOn";

        public string OrderType { get; set; } = "ASC";

        public int CompanyId { get; set; }

        public Enums.UserRole UserRole { get; set; }

        public bool IsFromMyProfile { get; set; } = false;
    }
}
