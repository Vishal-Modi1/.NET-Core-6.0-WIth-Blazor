using DataModels.VM.Aircraft;
using DataModels.VM.AircraftEquipment;
using DataModels.VM.Common;
using DataModels.VM.Reservation;
using DataModels.VM.User;
using DataModels.VM.UserRolePermission;
using Radzen;

namespace FSM.Blazor.Extensions
{
    public static class DataGridFilterCreator
    {
        public static DatatableParams Create(this DatatableParams datatableParam, LoadDataArgs args, string defaultSearchOrderColumn)
        {
            DatatableParams datatableParams = new DatatableParams();

            datatableParams.Length = args.Top.GetValueOrDefault();
            datatableParams.Start = args.Skip.GetValueOrDefault() + 1;

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

        public static UserDatatableParams Create(this UserDatatableParams userDatatableParams, LoadDataArgs args, string defaultSearchOrderColumn)
        {
            UserDatatableParams datatableParams = new UserDatatableParams();

            datatableParams.Length = args.Top.GetValueOrDefault();
            datatableParams.Start = args.Skip.GetValueOrDefault() + 1;

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

        public static AircraftDatatableParams Create(this AircraftDatatableParams aircraftDatatableParams, LoadDataArgs args, string defaultSearchOrderColumn)
        {
            AircraftDatatableParams datatableParams = new AircraftDatatableParams();

            datatableParams.Length = args.Top.GetValueOrDefault();
            datatableParams.Start = args.Skip.GetValueOrDefault() + 1;

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

        public static UserRolePermissionDatatableParams Create(this UserRolePermissionDatatableParams userRolePermissionDatatableParams, LoadDataArgs args, string defaultSearchOrderColumn)
        {
            UserRolePermissionDatatableParams datatableParams = new UserRolePermissionDatatableParams();

            datatableParams.Length = args.Top.GetValueOrDefault();
            datatableParams.Start = args.Skip.GetValueOrDefault() + 1;

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

        public static AircraftEquipmentDatatableParams Create(this AircraftEquipmentDatatableParams equipmentdatatableParams, LoadDataArgs args, string defaultSearchOrderColumn)
        {
            AircraftEquipmentDatatableParams datatableParams = new AircraftEquipmentDatatableParams();

            datatableParams.Length = args.Top.GetValueOrDefault();
            datatableParams.Start = args.Skip.GetValueOrDefault() + 1;

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

        public static ReservationDataTableParams Create(this ReservationDataTableParams reservationDataTableParams, LoadDataArgs args, string defaultSearchOrderColumn)
        {
            ReservationDataTableParams datatableParams = new ReservationDataTableParams();

            datatableParams.Length = args.Top.GetValueOrDefault();
            datatableParams.Start = args.Skip.GetValueOrDefault() + 1;

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
