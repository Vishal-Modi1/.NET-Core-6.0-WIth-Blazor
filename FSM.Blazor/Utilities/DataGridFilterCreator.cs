using DataModels.VM.Common;
using Radzen;

namespace FSM.Blazor.Utilities
{
    public static class DataGridFilterCreator
    {
        public static DatatableParams Create(LoadDataArgs args, string defaultSearchOrderColumn)
        {
            DatatableParams datatableParams = new DatatableParams();

            datatableParams.Length = (int)args.Top;
            datatableParams.Start = (int)args.Skip + 1;

            if (string.IsNullOrWhiteSpace(args.OrderBy))
            {
                datatableParams.SortOrderColumn = defaultSearchOrderColumn;
                datatableParams.OrderType = "asc";

            }
            else
            {
                datatableParams.SortOrderColumn = args.OrderBy.Split(new char[] { ' ' })[0];
                datatableParams.OrderType = args.OrderBy.Split(new char[] { ' ' })[1];
            }

            return datatableParams;
        }
    }
}
