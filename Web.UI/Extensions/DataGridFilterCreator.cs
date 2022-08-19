using DataModels.VM.Common;
using Telerik.Blazor.Components;

namespace Web.UI.Extensions
{
    public static class DataGridFilterCreator
    {
        //TODO: Datatables
        public static DatatableParams Create(this DatatableParams datatableParam, GridReadEventArgs args, string defaultSearchOrderColumn)
        {
            DatatableParams datatableParams = new DatatableParams();

            datatableParams.Length = args.Request.PageSize;
            datatableParams.Start = args.Request.Page;
            datatableParams.SortOrderColumn = defaultSearchOrderColumn;
            datatableParams.OrderType = "asc";

            if (args.Request.Sorts.Count > 0)
            {
                datatableParams.SortOrderColumn = args.Request.Sorts[0].Member;

                if (args.Request.Sorts[0].SortDirection == Telerik.DataSource.ListSortDirection.Ascending)
                {
                    datatableParams.OrderType = "asc";
                }
                else
                {
                    datatableParams.OrderType = "desc";
                }
            }

            return datatableParams;
        }
    }
}
